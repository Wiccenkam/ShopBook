using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBook
{
    public class OrderDelivery
    {
        public string UniqueCode { get; }
        public string Description { get; }
        public decimal Price { get; }
        public IReadOnlyDictionary<string,string> Parameters { get; }
        public OrderDelivery(string uniquecode, string description, IReadOnlyDictionary<string,string> parameters, decimal amount)
        {
            if (string.IsNullOrEmpty(uniquecode))
                throw new ArgumentException(nameof(uniquecode));
            if (string.IsNullOrEmpty(description))
                throw new ArgumentException(nameof(description));
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            UniqueCode = uniquecode;
            Description = description;
            Parameters = parameters;
            Price = amount;
        }
    }
}
