﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBook
{
    public interface IBookRepository
    {
        Book[] GetAllByTitleOrAuthor(string titlePart);
        Book[] GetAllByIsbn(string isbn);
    }
}
