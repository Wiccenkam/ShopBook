using ShopBook;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    public class StubBookRepository
    {
        public Book[] ResultOfGetByAllByIsb { get; set; }
        public Book[] ResultGetByTitleOrAuthor { get; set; }
        public Book[] GetAllByIds(IEnumerable<int> bookids)
        {
            throw new System.NotImplementedException();
        }
    }
}
