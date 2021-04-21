using System;
using System.Collections.Generic;
using System.Text;
using ShopBook;
using ShopBook.Contractors;
using Store.Web.Contractors;
using Microsoft.AspNetCore.Http;

namespace Store.Contractors.YandexKassa
{
    public class YandexKassaPaymentService : IPaymentService, IWebContractorService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        private HttpRequest Request => httpContextAccessor.HttpContext.Request;
        public YandexKassaPaymentService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public string Name => "YandexKassa";

        public string Title => "Payment by card";

        public string GetUri => "/YandexKassa/";

        public Form FirstForm(Order order)
        {
            return  Form.CreateFirst(Name).AddParameter("orderId",order.Id.ToString());
        }

        public OrderPayment GetPayment(Form form)
        {
            if (form.ServiceName != Name || !form.IsFinalStep)
                throw new InvalidOperationException("Invalid payment form");
            return new OrderPayment( Name,"Payment by card", form.Parameters);
        }

        public Form MoveNextForm( int step, IReadOnlyDictionary<string, string> values)
        {
            if (step != 1)
                throw new InvalidOperationException("Invalid Yandex.Kassa payment step");
            return  Form.CreateLast(Name,step+1,values);
        }

        public Uri StartSession(IReadOnlyDictionary<string,string> parameters, Uri returnUri)
        {
            var queryString = QueryString.Create(parameters);
            queryString += QueryString.Create("returnUri", returnUri.ToString());

            var builder = new UriBuilder(Request.Scheme, Request.Host.Host)
            {
                Path = "YandexKassa/",
                Query = queryString.ToString(),
            };
            if (Request.Host.Port != null)
                builder.Port = Request.Host.Port.Value;
            return builder.Uri;
        }
    }
}
