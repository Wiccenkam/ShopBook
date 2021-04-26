using ShopBook.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopBook
{
    public class OrderItemCollection : IReadOnlyCollection<OrderItem> 
    {
        private readonly OrderDto orderDto; 
        private readonly List<OrderItem> items;
        public OrderItemCollection(OrderDto orderDto)
        {
            if (orderDto == null)
                throw new ArgumentNullException(nameof(orderDto));
            this.orderDto = orderDto;
            items = orderDto.ItemsDtos.Select(OrderItem.Mapper.Map).ToList();
        }

        public int Count => items.Count;
        public OrderItem Get(int bookId)
        {
            if (TryGet(bookId, out OrderItem orderItem))
                return orderItem;
            throw new InvalidOperationException("Book not found");
        }
        public OrderItem Add (int bookId, decimal price, int count)
        {
            if (TryGet(bookId, out OrderItem orderItem))
                throw new InvalidOperationException("Book already exist");
            var orderItemDto = OrderItem.DtoFactory.Create(orderDto, bookId, price, count);
            orderDto.ItemsDtos.Add(orderItemDto);
            orderItem = OrderItem.Mapper.Map(orderItemDto);
            items.Add(orderItem);
            return orderItem;


        }
        public bool TryGet(int bookId,out OrderItem orderItem)
        {
            var index = items.FindIndex(item => item.BookId == bookId);
            if (index == -1)
            {
                orderItem = null;
                return false; 
            }
            orderItem = items[index];
            return true;
        }
        public void Remove(int bookId)
        {
            var index = items.FindIndex(item => item.BookId == bookId);
            if (index == -1)
                throw new InvalidOperationException("Cant find book to remove from order");
            orderDto.ItemsDtos.RemoveAt(index);
            items.RemoveAt(index);
        }

        public IEnumerator<OrderItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (items as IEnumerable).GetEnumerator();
        }

    }

    
    
}
