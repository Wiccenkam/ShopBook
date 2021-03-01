using ShopBook;
using System;
using System.Linq;

namespace Store.Memory
{
    public class BookRepositiry : IBookRepository
    {
        private readonly Book[] books = new[]
        {
            new Book(1,"Isbn 24081-77758","D.Knuth","Art of Programing","According to Webster's Dictionary, a fascicle is one of the division of a bookpublished in parts. This material represents significant updates to Volume 1, Third Edition ofDonald Knuth's The Art of Computer Programming.Knuth's fascicle philosophy is as follows: The material will first appear in betatestform as fascicles of approximately 128 pages each, issued approximatelytwice per year. These fascicles will represent my best attempt to write acomprehensive account, but computer science has grown to the point where Icannot hope to be an authority on all the material covered in these books",29.99m),
            new Book(2,"Isbn 24081-99958","Jeffrey Richter","Clr via C#","Dig deep and master the intricacies of the common language runtime, C#, and .NET development. Led by programming expert Jeffrey Richter, a longtime consultant to the Microsoft .NET team - you’ll gain pragmatic insights for building robust, reliable, and responsive apps and components",58.88m),
            new Book(3,"Isbn 24081-88858","M.Fowler","Refactoring","For more than twenty years, experienced programmers worldwide have relied on Martin Fowler’s Refactoring to improve the design of existing code and to enhance software maintainability, as well as to make existing code easier to understand",50.45m),
        };

        public Book[] GetAllByIsbn(string isbn)
        {
            return books.Where(book => book.Isbn == isbn).ToArray();
        }

        public Book[] GetAllByTitleOrAuthor(string titlePart)
        {
          
                return books.Where(book => book.Author.Contains(titlePart) || book.Title.Contains(titlePart)).ToArray();

           
        }

        public Book GetById(int id)
        {
            return books.Single(book => book.Id == id);
        }
    }
}
