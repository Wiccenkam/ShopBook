using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreBook.Web.Models
{
    public class Cart
    {
        public int OrderId { get; }
        public int TotalCount { get; set; }
        public decimal TotalPrice { get; set;}
            

        public Cart (int orderID)
        {
            OrderId = orderID;
            TotalCount = 0;
            TotalPrice = 0m;
        }
    }
}
