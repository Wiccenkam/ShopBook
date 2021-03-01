using Moq;
using ShopBook;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests
{
    public class BookServiceTests
    {
        [Fact]
        public void GetAllByQuery_WithCallsGetByIsbn()
        {
            var bookRepositoryStub = new Mock<IBookRepository>();

            bookRepositoryStub.Setup(x => x.GetAllByIsbn(It.IsAny<string>())).Returns(new[] { new Book(1, "", "", "","",0m) });

            bookRepositoryStub.Setup(x => x.GetAllByTitleOrAuthor(It.IsAny<string>())).Returns(new[] { new Book(2, "", "", "","", 0m) });

            var bookService = new BookService(bookRepositoryStub.Object);

            var validIsbn = "ISBN 12345-67890";

            var actual = bookService.GetAllByQuery(validIsbn);

            Assert.Collection(actual, book => Assert.Equal(1,book.Id));
        }
        [Fact]
        public void GetAllByQuery_WithAuthor()
        {
            var bookRepositoryStub = new Mock<IBookRepository>();

            bookRepositoryStub.Setup(x => x.GetAllByIsbn(It.IsAny<string>())).Returns(new[] { new Book(1, "", "", "","", 0m) });

            bookRepositoryStub.Setup(x => x.GetAllByTitleOrAuthor(It.IsAny<string>())).Returns(new[] { new Book(2, "", "", "", "", 0m) });

            var bookService = new BookService(bookRepositoryStub.Object);

            var invalidIsnb = "12345-67890";

            var actual = bookService.GetAllByQuery(invalidIsnb);

            Assert.Collection(actual, book => Assert.Equal(2, book.Id));
        }
    }
}
