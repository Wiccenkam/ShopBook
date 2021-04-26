using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBook.Data
{
   public  class OrderItemDto
   {
        public int Id { get; set; }
        public int BookId { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public OrderDto Order { get; set; }

    }
}
