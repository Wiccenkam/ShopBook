using System;
using System.Collections.Generic;
using System.Text;
using ShopBook;

namespace Store.Data.EF
{
    class BookRepository : IBookRepository
    {
        public Book[] GetAllByIsbn(string isbn)
        {
            throw new NotImplementedException();
        }

        public Book[] GetAllByTitleOrAuthor(string titlePart)
        {
            throw new NotImplementedException();
        }

        public Book GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Book[] GetByIds(IEnumerable<int> bookIds)
        {
            throw new NotImplementedException();
        }
    }
}
