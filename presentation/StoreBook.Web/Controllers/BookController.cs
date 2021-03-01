using Microsoft.AspNetCore.Mvc;
using ShopBook;
using Store.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreBook.Web.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository bookRepository;
        public BookController(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }
        public IActionResult Index(int id)
        {
            Book book =  bookRepository.GetById(id);
            return View(book);
        }
    }
}
