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
        public string UniqueCode => "Postamate";
        public string Title => "Delivery from Moscow to St.Petersberg";

        public OrderDelivery CreateDelivery(Form formDelivery)
        {
            if (formDelivery.UniqueCode != UniqueCode || !formDelivery.IsFinalStep)
            {
                throw new InvalidOperationException("Invalid form.");
            }
            var cityId = formDelivery.Fields.Single(field => field.Name == "City").Value;
            var cityName = cities[cityId];
            var postamateId = formDelivery.Fields.Single(field => field.Name == "postamate").Value;
            var postamateName = postamates[cityId][postamateId];
            var parameters = new Dictionary<string, string>
            {
                   {nameof(cityId), cityId },
                   {nameof(cityName), cityName },
                   {nameof(postamateId), postamateId },
                   {nameof(postamateName), postamateName }
            };
            var description = $"City {cityName}\nPostamate: {postamateName}";
            return new OrderDelivery(UniqueCode, description, parameters, 150m);
        }

        public Form CreateForm(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            return new Form(UniqueCode, order.Id, 1, false, new[]
            {
                new SelectionField("City","City","1",cities)
            });
        }
        public Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> values)
        {
            if (step == 1)
            {
                if (values["City"].Equals("1"))
                {
                    return new Form(UniqueCode, orderId, 2, false, new Field[]
                    {
                        new HiddenField("City", "City", "1"),
                        new SelectionField("Postamate", "postamate", "1", postamates["1"])
                    });
                }
                else if (values["City"].Equals("2"))
                {
                    return new Form(UniqueCode, orderId, 2, false, new Field[]
                    {
                        new HiddenField("City", "City", "2"),
                        new SelectionField("Postamate", "postamate", "4",  postamates["2"])
                    });
                }
                else
                {
                    throw new InvalidOperationException("Invalid postamate");
                }
            }
            else
            {
                if (step == 2)
                {
                    return new Form(UniqueCode, orderId, 3, true, new Field[]
                   {
                            new HiddenField("City", "City", values["City"]),
                            new HiddenField("Postamate", "postamate", values["postamate"])
                   });
                }
                else
                {
                    throw new InvalidOperationException("Invalid postamate");
                }
            }
        }
    }
}
