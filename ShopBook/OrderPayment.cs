using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBook
{
    public class OrderPayment
    {
        public string UniqueCode { get; }
        public string Description { get; }
        
        public IReadOnlyDictionary<string,string> Parameters { get; }
        public OrderPayment(string uniquecode, string description, IReadOnlyDictionary<string,string> parameters)
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
        }
    }
}
