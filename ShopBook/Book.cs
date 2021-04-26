using ShopBook.Data;
using System;
using System.Text.RegularExpressions;

namespace ShopBook
{
    public class Book
    {
        private readonly BookDto dto;
        public int Id => dto.Id;
        public string Title 
        {
            get => dto.Title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(Title));
                dto.Title = value.Trim();
            }
        }
        public string Isbn 
        {
            get => dto.Isbn;
            set
            {
                if (TryFormattedIsbn(value, out string formattedIsbn))
                    dto.Isbn = formattedIsbn;
                throw new ArgumentException(nameof(Isbn));
            }
        }
        public string Author 
        {
            get => dto.Author;
            set => dto.Author = value?.Trim();
        }
        public string Description 
        {
            get => dto.Description;
            set => dto.Description = value;
        }
        public decimal Price
        {
            get => dto.Price;
            set => dto.Price = value;
        }
        internal Book(BookDto dto)
        {
            this.dto = dto;
        }
        public static bool IsIsbn(string isbn) =>
            TryFormattedIsbn(isbn, out _);
        
        public static bool TryFormattedIsbn(string isbn, out string formattedIsbn)
        {
            if (isbn == null)
            {
                formattedIsbn = null;
                return false;
            }
            formattedIsbn = isbn.Replace("-", "")
                            .Replace(" ", "")
                            .ToUpper();
            return Regex.IsMatch(formattedIsbn, @"^ISBN\d{10}(\d{3})?$");
        }
        public static class DtoFactory
        {
          public static BookDto Create(string isbn, string author,string title,string description,decimal price)
            {
                if (TryFormattedIsbn(isbn, out string formattedIsbn))
                    isbn = formattedIsbn;
                else
                    throw new ArgumentException(nameof(isbn));
                if (string.IsNullOrWhiteSpace(title))
                    throw new ArgumentException(nameof(title));
                return new BookDto
                {
                    Isbn = isbn,
                    Author = author?.Trim(),
                    Title = title.Trim(),
                    Description = description?.Trim(),
                    Price = price,
                };
            }
        }
        public static class Mapper
        {
            public static Book Map(BookDto dto) => new Book(dto);
            public static BookDto Mapp(Book domain) => domain.dto;
        }

    }
}
