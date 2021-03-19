using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopBook
{
    public class Order
    {
        public int Id { get; }
        private List<OrderItem> items;
        public IReadOnlyCollection<OrderItem> Items
        {
            get { return items; }
        }
        public int TotalCount => items.Sum(items => items.Count);
      
        public decimal TotalPrice
        {
            get { return items.Sum(items => items.Price * items.Count); }
        }
        public Order(int id, IEnumerable<OrderItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            Id = id;
            this.items = new List<OrderItem>(items);
        }
   
        public OrderItem GetItem(int bookid)
        {
            int index = items.FindIndex(item => item.BookId == bookid);
            if(index==-1)
                ThrowBookException("Book not found",bookid);
            return items[index];
        }
        public bool ContainsItem(int bookId)
        {
            return items.Any(item => item.BookId == bookId);
        }
        public  void AddOrUpdateItem(Book book, int count)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));
            
            int index = items.FindIndex(item => item.BookId == book.Id);
            if (index == -1)
            {
                items.Add(new OrderItem(book.Id, count, book.Price));
            }
            else
            {
                items[index].Count += count;
            }
        }

        public void RemoveItem(int book)
        {
            
            int index = items.FindIndex(item => item.BookId == book);
            if (index == -1)
                 ThrowBookException("Order does not contain specified book.", book);
            items.RemoveAt(index);
        }
        private void ThrowBookException(string message, int book)
        {
            var exception = new InvalidOperationException(message);
            exception.Data["BookId"] = book;
            

            throw exception;
        }
    }
}
