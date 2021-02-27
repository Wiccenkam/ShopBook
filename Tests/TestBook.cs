using ShopBook;
using System;
using Xunit;

namespace Tests
{
    public class TestBook
    {
        [Fact]
        public void IsIsbn_With_Null()
        {
            bool actual = Book.IsIsbn(null);
            Assert.False(actual);
        }
        [Fact]
        public void IsIsbn_With_WhiteSpace()
        {
            bool actual = Book.IsIsbn("  ");
            Assert.False(actual);
        }

        [Fact]
        public void IsIsbn_With_InvaidCast()
        {
            bool actual = Book.IsIsbn("Isbn 123 ");
            Assert.False(actual);
        }
        [Fact]
        public void IsIsbn_With_10Numbers()
        {
            bool actual = Book.IsIsbn("Isbn 444-555-666 0");
            Assert.True(actual);
        }
        [Fact]
        public void IsIsbn_With_10MoreNumbers()
        {
            bool actual = Book.IsIsbn(" Isbn 444-555-666-1230 ");
            Assert.True(actual);
        }
        [Fact]
        public void IsIsbn_With_InvalidStart()
        {
            bool actual = Book.IsIsbn("xxx Isbn 444-555-666-1230 xxx");
            Assert.False(actual);
        }
    }
}
