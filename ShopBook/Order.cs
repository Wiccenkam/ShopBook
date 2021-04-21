using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopBook
{
    public class Order
    {
        public int Id { get; }
        
        public string CellPhone { get; set; }
        public OrderDelivery Delivery { get; set; }
        public OrderPayment Payment { get; set; }
        public OrderItemCollection Items { get; }
      
        public int TotalCount => Items.Sum(items => items.Count);
      
        public decimal TotalPrice
        {
            get { return Items.Sum(items => items.Price * items.Count)
                    + (Delivery?.Amount??0m); }
        }
        public Order(int id, IEnumerable<OrderItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            Id = id;
            Items = new OrderItemCollection(items);
        }
   
        
    }
}
