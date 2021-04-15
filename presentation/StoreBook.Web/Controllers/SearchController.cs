using Microsoft.AspNetCore.Mvc;
using ShopBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Web.Application
{
    public class SearchController : Controller
    {
        private readonly BookService bookService;
        
        public SearchController(BookService bookService)
        {
            this.bookService = bookService;
        }
        public IActionResult Index(string query)
        {
            var books = bookService.GetAllByQuery(query);
            return View("Index", books);
        }
    }
}
