using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using PhoneNumbers;
using ShopBook;
using ShopBook.Messages;

namespace Store.Web.Application
{
    public class OrderService
    {
        private readonly IBookRepository bookRepository;
        private readonly IOrderRepository orderRepository;
        private readonly INotificationService notificationService;
        private readonly IHttpContextAccessor httpContextAccessor;

        protected ISession Session => httpContextAccessor.HttpContext.Session;

        public OrderService(IBookRepository bookRepository,
                            IOrderRepository orderRepository,
                            INotificationService notificationService,
                            IHttpContextAccessor httpContextAccessor)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.notificationService = notificationService;
            this.httpContextAccessor = httpContextAccessor;
        }

        internal bool TryGetOrder(out Order order)
        {
            if (Session.TryGetCart(out Cart cart))
            {
                order = orderRepository.GetById(cart.OrderId);
                return true;
            }
            order = null;
            return false;
        }
        public bool TryGetModel(out OrderModel model)
        {
            if(TryGetOrder(out Order order))
            {
                model = Map(order);
                return true;
            }
            model = null;
            return false;
        }

        internal OrderModel Map(Order order)
        {
            var books = GetBooks(order);
            var items = from item in order.Items
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
                Items = items.ToArray(),
                TotalCount = order.TotalCount,
                TotalPrice = order.TotalPrice,
                CellPhone = order.CellPhone,
                DeliveryDescription = order.Delivery?.Description,
                PaymentDescription = order.Payment?.Description
            };
        }
        internal IEnumerable<Book> GetBooks(Order order)
        {
            var bookIds = order.Items.Select(item => item.BookId);
            return bookRepository.GetByIds(bookIds); 
        }
        public OrderModel AddBook(int bookid, int count)
        {
            if (count < 1)
                throw new InvalidOperationException("Too few books to add");
            if (!TryGetOrder(out Order order))
                order = orderRepository.Create();
            
             AddOrUpdateBook(order, bookid, count);
             UpdateSession(order);
            
            return Map(order);
        }
        internal void AddOrUpdateBook(Order order,int bookid, int count)
        {
            var book = bookRepository.GetById(bookid);
            if (order.Items.TryGet(bookid, out OrderItem orderItem))
                orderItem.Count += count;
            else
                order.Items.Add(book.Id, book.Price, count);
        }
        internal void UpdateSession(Order order)
        {
            var cart = new Cart(order.Id, order.TotalCount, order.TotalPrice);
            Session.Set(cart);
        }
        public OrderModel UpdateBook(int bookid,int count)
        {
            var order = GetOrder();
            order.Items.Get(bookid).Count = count;
            orderRepository.Update(order);
            UpdateSession(order);
            return Map(order);
        }
        public OrderModel RemoveBook(int bookid)
        {
            var order = GetOrder();
            order.Items.Remove(bookid);
            orderRepository.Update(order);
            UpdateSession(order);
            return Map(order);
        }
        public Order GetOrder()
        {
            if(TryGetOrder(out Order order))
                return order;
            throw new InvalidOperationException("Empty Session");
        }
        public OrderModel SendConfirmation(string CellPhone)
        {
            var order = GetOrder();
            var model = Map(order);
            if (TryFormatPhone(CellPhone, out string FormattedPhone))
            {
                var confirmationCode = 1111; //random.Next(1000,1000)=
                model.CellPhone = FormattedPhone;
                Session.SetInt32(FormattedPhone, confirmationCode);
                notificationService.SendConfirmationCode(FormattedPhone, confirmationCode);
            }
            else
                model.Errors["CellPhone"] = "Phone number does not match";
            return model;
        }
        private readonly PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
        internal bool TryFormatPhone(string cellPhone, out string formattedPhone)
        {
            try
            {
                var phoneNumber = phoneNumberUtil.Parse(cellPhone, "ru");
                formattedPhone = phoneNumberUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL);
                return true;
            }
            catch (NumberParseException)
            {
                formattedPhone = null;
                return false;
            }
        }
        public OrderModel ConfirmCellPhone(string cellPhone, int confirmationCode)
        {
            int? storedCode = Session.GetInt32(cellPhone);
            var model = new OrderModel();
            if(storedCode == null)
            {
                model.Errors["CellPhone"] = "Something wrong. try get phone code again";
                return model;
            }
            if (storedCode != confirmationCode)
            {
                model.Errors["CellPhone"] = "Invalid phone code";
                return model;
            }
            var order = GetOrder();
            order.CellPhone = cellPhone;
            orderRepository.Update(order);
            Session.Remove(cellPhone);
            return Map(order);
        }
        public OrderModel SetDelivery(OrderDelivery delivery)
        {
            var order = GetOrder();
            order.Delivery = delivery;
            orderRepository.Update(order);
            return Map(order);
        }
        public OrderModel SetPayment(OrderPayment payment)
        {
            var order = GetOrder();
            order.Payment = payment;
            orderRepository.Update(order);
            Session.RemoveCart();
            return Map(order);
        }
    }
}
