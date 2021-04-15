using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Web.Application
{
    public class Cart
    {
        public int OrderId { get; }
        public int TotalCount { get; }
        public decimal TotalPrice { get; }
            

        public Cart (int orderID, int totalCount, decimal totalPrice)
        {
            OrderId = orderID;
            TotalCount = totalCount;
            TotalPrice = totalPrice;
        }
    }
}
