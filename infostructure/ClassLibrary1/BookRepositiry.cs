using ShopBook;
using System;
using System.Linq;

namespace Store.Memory
{
    public class BookRepositiry : IBookRepository
    {
        private readonly Book[] books = new[]
        {
            new Book(1,"Isbn 24081-77758","D.Knuth","Art of Programing"),
            new Book(2,"Isbn 24081-99958","Jeffrey Richter","Clr via C#"),
            new Book(3,"Isbn 24081-88858","M.Fowler","Refactoring"),
        };

        public Book[] GetAllByIsbn(string isbn)
        {
            return books.Where(book => book.Isbn == isbn).ToArray();
        }

        public Book[] GetAllByTitleOrAuthor(string titlePart)
        {
            return books.Where(book => book.Author.Contains(titlePart)||book.Title.Contains(titlePart)).ToArray();
           
        }
    }
}
