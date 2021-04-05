using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopBook;
using ShopBook.Contractors;
using ShopBook.Messages;
using StoreBook.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StoreBook.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBookRepository bookRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IEnumerable<IDeliveryService> deliveryServices;
        private readonly IEnumerable<IPaymentService> paymentServices;
        private readonly INotificationService notificationService;
        public OrderController(IBookRepository bookRepository, IOrderRepository orderRepository, IEnumerable<IDeliveryService> deliveryServices,IEnumerable<IPaymentService> paymentServices, INotificationService notificationService)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.deliveryServices = deliveryServices;
            this.paymentServices = paymentServices;
            this.notificationService = notificationService;
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
        public IActionResult AddItem(int id, int count=1)//Ошибка представления из за анонимного метода 
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();
            var book = bookRepository.GetById(id);
            order.AddOrUpdateItem(book,count);
            SaveOrderAndCart(order, cart);
            return RedirectToAction("Index", "Book", new { id  });
           
        }
        [HttpPost]
        public IActionResult RemoveItem(int bookid)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();
            order.RemoveItem(bookid);
            SaveOrderAndCart(order, cart);
            return RedirectToAction("Index", "Order");
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
        [HttpPost]
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
        [HttpPost]
        public IActionResult SendConfirmationCode(int orderId, string cellPhone)
        {
            var order = orderRepository.GetById(orderId);
            var model = Map(order);

            if (!IsValidCellPhone(cellPhone))
            {
                model.Errors["cellPhone"] = "Phone number must been in fromat +381234567890";
                return View("Index", model);
            }
            int code = 1111;
            HttpContext.Session.SetInt32(cellPhone, code);
            notificationService.SendConfirmationCode(cellPhone, code);

            return View("Confirmation",
               new ConfirmationModel
               {
                   OrderId = orderId,
                   CellPhone = cellPhone
               });
        }

        private bool IsValidCellPhone(string cellPhone)
        {
            if (cellPhone == null)
            {
                return false;
            }

            cellPhone = cellPhone.Replace(" ", "").Replace("-", "");
            return Regex.IsMatch(cellPhone, @"^\+?\d{12}$");
        }
        [HttpPost]
        public IActionResult Confirmate (int orderId, string cellPhone, int code)
        {
            
            int? storeCode = HttpContext.Session.GetInt32(cellPhone);
            var order = orderRepository.GetById(orderId);
            order.CellPhone = cellPhone;
            orderRepository.Update(order);
            HttpContext.Session.Remove(cellPhone);
            var model = new DeliveryModel
            {
                OrderId = orderId,
                Methods = deliveryServices.ToDictionary(service => service.UniqueCode, service => service.Title)
            };

            return View("DeliveryMethod", model);

            if (storeCode == null)
            {
                return View("Confirmation", new ConfirmationModel
                {
                    OrderId = orderId,
                    CellPhone = cellPhone,
                    Errors = new Dictionary<string, string>
                    {
                        {"code", "Empty confirm code. Repeat again" }
                    }
                });
            }
            if (storeCode != code)
            {
                return View("Confirmation",
                     new ConfirmationModel
                     {
                         OrderId = orderId,
                         CellPhone = cellPhone,
                         Errors = new Dictionary<string, string>
                         {
                            {"code", " is defferent from the one sent" }
                         }
                     });
            }
           
        }
        [HttpPost]
        public IActionResult StartDelivery(int id, string uniqueCode)
        {
            var deliverService = deliveryServices.Single(service => service.UniqueCode == uniqueCode);
            var order = orderRepository.GetById(id);
            var form = deliverService.CreateForm(order);
            return View("DeliveryStep", form);
        }
        [HttpPost]
        public IActionResult NextDelivery(int id, string uniqueCode, int step, Dictionary<string,string> values)
        {
            var deliverService = deliveryServices.Single(service => service.UniqueCode == uniqueCode);
            var form = deliverService.MoveNext(id, step, values);
            if (form.IsFinalStep)
            {
                var order = orderRepository.GetById(id);
                order.Delivery = deliverService.CreateDelivery(form);
                orderRepository.Update(order);
                var model = new DeliveryModel
                {
                    OrderId = id,
                    Methods = paymentServices.ToDictionary(service => service.UniqueCode, service => service.Title)
                };
                return View("PaymentMethod", model);
            }
            return View("DeliveryStep", form);
        }
    }
}
