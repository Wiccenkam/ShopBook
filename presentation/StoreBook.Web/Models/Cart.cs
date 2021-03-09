using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreBook.Web.Models
{
    public class Cart
    {
        public IDictionary<int, int> Items = new Dictionary<int, int>();
        public decimal Amount { get; set; }
    }
}
