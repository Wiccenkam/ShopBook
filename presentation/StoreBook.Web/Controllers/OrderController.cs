using Microsoft.AspNetCore.Mvc;
using ShopBook;
using StoreBook.Web.Models;
using System.Linq;


namespace StoreBook.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBookRepository bookRepository;
        private readonly IOrderRepository orderRepository;
        public OrderController(IBookRepository bookRepository, IOrderRepository orderRepository)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                var order = orderRepository.GetById(cart.OrderId);
                OrderModel model = Map(order);
                return View(model);
            }
            return View("Empty");
        }
        public IActionResult AddItem(int bookid, int count)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();
            var book = bookRepository.GetById(bookid);
            order.AddOrUpdateItem(book,count);
            SaveOrderAndCart(order, cart);
            return RedirectToAction("Index", "Book", new { bookid });
           
        }
        public IActionResult RemoveItem(int bookid)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();
            order.RemoveItem(bookid);
            SaveOrderAndCart(order, cart);
            return RedirectToAction("Index", "Book", new { bookid });
        }
        private (Order order,Cart cart ) GetOrCreateOrderAndCart()
        {
            Order order;
            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                order = orderRepository.GetById(cart.OrderId);
            }
            else
            {
                order = orderRepository.Create();
                cart = new Cart(order.Id);
            }
            return (order, cart);
        }
        public IActionResult RemoveBook(int bookid)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();
            order.GetItem(bookid).Count--;
            SaveOrderAndCart(order, cart);
            return RedirectToAction("Index", "Book", new { bookid });
        }
        public IActionResult UpdateItem(int bookid, int count)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();
            order.GetItem(bookid).Count= count;
            SaveOrderAndCart(order, cart);
            return RedirectToAction("Index", "Book", new { bookid });

        }

        private void SaveOrderAndCart(Order order, Cart cart)
        {
            orderRepository.Update(order);

            cart.TotalCount = order.TotalCount;
            cart.TotalPrice = order.TotalPrice;
            HttpContext.Session.Set(cart);
        }
        

        private OrderModel Map(Order order)
        {
            var bookIds = order.Items.Select(item => item.BookId);
            var books = bookRepository.GetByIds(bookIds);
            var itemModels = from item in order.Items
                             join book in books on item.BookId equals book.Id
                             select new OrderItemModel
                             {
                                 BookId = book.Id,
                                 Title = book.Title,
                                 Author = book.Author,
                                 Price = item.Price,
                                 Count = item.Count,
                                 
                             };
            return new OrderModel
            {
                Id = order.Id,
                Items = itemModels.ToArray(),
                TotalCount = order.TotalCount,
                TotalPrice = order.TotalPrice,
            };
        }
    }
}
