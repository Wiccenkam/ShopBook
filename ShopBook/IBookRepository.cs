using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBook
{
    public interface IBookRepository
    {
        Book[] GetAllByTitle(string titlePart);
    }
}
