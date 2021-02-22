using ShopBook;
using System;
using System.Linq;

namespace Store.Memory
{
    public class BookRepositiry : IBookRepository
    {
        private readonly Book[] books = new[]
        {
            new Book(1,"Art of Programing"),
            new Book(2,"Clr via C#"),
            new Book(3,"Refactoring"),
        };
        public Book[] GetAllByTitle(string titlePart)
        {
            return books.Where(book => book.Title.Contains(titlePart)).ToArray();
           
        }
    }
}
