using ShopBook.Contractors;
using ShopBook;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System;

namespace store.Contractors
{
    public class PostamateDeliveryService : IDeliveryService
    {
        private static IReadOnlyDictionary<string, string> cities = new Dictionary<string, string>
        {
            {"1", "Moscow" },
            {"2", "St. Petersberg" }

        };
        private static IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> postamates = new Dictionary<string, IReadOnlyDictionary<string, string>>
        {
            {
                "1",
                new Dictionary<string, string>
                {
                    { "1", "Kazan Station"},
                    { "2", "Kiev Station"},
                    { "3", "Belarusskiy Station"},
                }
            },
            {
                "2",
                new Dictionary<string, string>
                {
                    { "4", "Moscow Station"},
                    { "5", "Finland Station"},
                    { "6", "Vitebsk Station"},
                }
            }
        };
        public string Name => "Postamate";
        public string Title => "Delivery from Moscow to St.Petersberg";

        public OrderDelivery GetDelivery(Form formDelivery)
        {
            if (formDelivery.ServiceName != Name || !formDelivery.IsFinalStep)
            {
                throw new InvalidOperationException("Invalid form.");
            }
            var cityId = formDelivery.Parameters["city"];
            var cityName = cities[cityId];
            var postamateId = formDelivery.Parameters["postamate"];
            var postamateName = postamates[cityId][postamateId];
            var parameters = new Dictionary<string, string>
            {
                   {nameof(cityId), cityId },
                   {nameof(cityName), cityName },
                   {nameof(postamateId), postamateId },
                   {nameof(postamateName), postamateName }
            };
            var description = $"City {cityName}\nPostamate: {postamateName}";
            return new OrderDelivery(Name, description, parameters, 150m);
        }

        public Form FirstForm(Order order)
        {
            return Form.CreateFirst(Name).AddParameter("orderId", order.Id.ToString()).
                AddField(new SelectionField("City", "city", "1", cities));
        }
        public Form MoveNextForm( int step, IReadOnlyDictionary<string, string> values)
        {
            if (step == 1)
            {
                if (values["city"].Equals("1"))
                {
                    return Form.CreateNext(Name, 2, values).AddField(new SelectionField("Postamate", "postamate", "1", postamates["1"]));
                }
                else if (values["city"].Equals("2"))
                {
                    return Form.CreateNext(Name, 2, values).AddField(new SelectionField("Postamate", "postamate", "4", postamates["2"]));
                }
                else
                {
                    throw new InvalidOperationException("Invalid postamate city");
                }
            }
            else if (step == 2)
            {
                return Form.CreateLast(Name, 3, values);
            }
            else
                throw new InvalidOperationException("InvalidPostamateStep");

        }
    }
}
