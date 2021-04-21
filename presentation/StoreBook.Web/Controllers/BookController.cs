using Microsoft.AspNetCore.Mvc;
using ShopBook;
using Store.Memory;
using Store.Web.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreBook.Web.Controllers
{
    public class BookController : Controller
    {
        private readonly BookService bookSerivce;
        public BookController(BookService bookSerivce)
        {
            this.bookSerivce = bookSerivce;
        }
        public IActionResult Index(int id)
        {
            var model = bookSerivce.GetById(id);

            return View(model);
        }
    }
}
