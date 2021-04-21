using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopBook;
using ShopBook.Contractors;
using ShopBook.Messages;
using StoreBook.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Store.Contractors.YandexKassa;
using Store.Web.Contractors;
using Store.Web.Application;
using System;

namespace StoreBook.Web.Controllers
{
    public class OrderController : Controller
    {
        
        private readonly IEnumerable<IDeliveryService> deliveryService;
        private readonly IEnumerable<IPaymentService> paymentService;
        private readonly OrderService orderService;
        private readonly IEnumerable<IWebContractorService> webContractorService;
        public OrderController( IEnumerable<IDeliveryService> deliveryService,IEnumerable<IPaymentService> paymentService,
                                OrderService orderService,
                                IEnumerable<IWebContractorService> webContractorService)
        {
            
            this.deliveryService = deliveryService;
            this.paymentService = paymentService;
            this.orderService = orderService;
            this.webContractorService = webContractorService;
        }
        public IActionResult Index()
        {
            if (orderService.TryGetModel(out OrderModel model))
                return View(model);
            return View("Empty");
          
        }
        public IActionResult AddItem(int id, int count=1)//Ошибка представления из за анонимного метода 
        {
            orderService.AddBook(id, count);
            return RedirectToAction("Index", "Book", new { id  });
           
        }
        [HttpPost]
        public IActionResult RemoveItem(int bookid)
        {
            var model = orderService.RemoveBook(bookid);
            return View("Index", model);
        }
        [HttpPost]
        public IActionResult UpdateItem(int bookid, int count)
        {
            var model = orderService.UpdateBook(bookid,count);
            return View("Index", model);
            //return RedirectToAction("Index", "Book", new { bookid });

        }
        [HttpPost]
        public IActionResult SendConfirmationCode(int orderId, string cellPhone)
        {
            var model = orderService.SendConfirmation(cellPhone);
            return View("Confirmation",model);
        }
        [HttpPost]
        public IActionResult ConfirmCellPhone ( string cellPhone, int confirmationCode)
        {

            var model = orderService.ConfirmCellPhone(cellPhone, confirmationCode);
            if (model.Errors.Count > 0)
                return View("Confirmation", model);
            var deliveryMethods = deliveryService.ToDictionary(serice => serice.Name,
                                                               service => service.Title);
            return View("DeliveryMethod", deliveryMethods);

        }
        [HttpPost]
        public IActionResult StartDelivery(string serviceName)
        {
            var deliverService = deliveryService.Single(service => service.Name == serviceName);
            var order = orderService.GetOrder();
            //var books = orderService.GetOrderBooks();
            var form = deliverService.FirstForm(order);

            var webContractorService = this.webContractorService.SingleOrDefault(service => service.Name==serviceName);
            if (webContractorService == null)
                return View("DeliveryStep", form);

            var returnUri = GetReturnUri(nameof(NextDelivery));
            var redirectUri = webContractorService.StartSession(form.Parameters, returnUri);

            return Redirect(redirectUri.ToString());
        }
        private Uri GetReturnUri(string action)
        {
            var builder = new UriBuilder(Request.Scheme, Request.Host.Host)
            {
                Path = Url.Action(action),
                Query = null
            };
            if (Request.Host.Port != null)
                builder.Port = Request.Host.Port.Value;
            return builder.Uri;
        }
        [HttpPost]
        public IActionResult NextDelivery( string serviceName, int step, Dictionary<string,string> values)
        {
            var deliverService = deliveryService.Single(service => service.Name == serviceName);
            var form = deliverService.MoveNextForm(step, values);
            if (!form.IsFinalStep)
                return View("DeliveryStep", form);

            var delivery = deliverService.GetDelivery(form);
            orderService.SetDelivery(delivery);
            var paymentMethods = paymentService.ToDictionary(service => service.Name,
                                                             service => service.Title);
            return View("PaymentMethod", paymentMethods); 
        }
        [HttpPost]
        public IActionResult StartPayment(string serviceName)
        {
            var paymentService = this.paymentService.Single(service => service.Name ==  serviceName);
            var order = orderService.GetOrder();
            var form = paymentService.FirstForm(order);

            var webContractorService = this.webContractorService.SingleOrDefault(service => service.Name == serviceName);
            if (webContractorService == null)
                return View("PaymentStep", form);
            var returnUri = GetReturnUri(nameof(NextPayment));
            var redirectUri = webContractorService.StartSession(form.Parameters, returnUri);
            return Redirect(redirectUri.ToString());
        }
        public IActionResult Finish()
        {
            HttpContext.Session.RemoveCart(); 
            return View("PaymentFinish");
        }
        [HttpPost]
        public IActionResult NextPayment(string serviceName, int step, Dictionary<string, string> values)
        {
            var paymentService = this.paymentService.Single(service => service.Name == serviceName);
            var form = paymentService.MoveNextForm(step, values);
            if (!form.IsFinalStep)
                return View("PaymentStep", form);

            var payment = paymentService.GetPayment(form);
            var model = orderService.SetPayment(payment);

            return View("PaymentFinish", model);
        }
    }
}
