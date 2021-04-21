using ShopBook;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests
{
    public class OrderItemTest
    {
        [Fact]
        public void OrderItem_WithZeroCount_ThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = 0;
                new OrderItem(1, 0m, count);
            });
        }
        [Fact]
        public void OrderItem_WithNEgativeCount_ThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = -1;
                new OrderItem(1, 0m, count);
            });
        }
        [Fact]
        public void OrderItem_WithPositiveCount_SetCount()
        {
            var orderItem = new OrderItem(1, 3m, 2);
            Assert.Equal(1,orderItem.BookId);
            Assert.Equal(2, orderItem.Count);
            Assert.Equal(3m, orderItem.Price);
            
        }
        [Fact]
        public void Count_With_NegativeValue_ThrowArgumentOfRange()
        {
            var orderItem = new OrderItem(0, 0m, 2);
            Assert.Throws<ArgumentOutOfRangeException>(() => orderItem.Count = -1);

        }
        [Fact]
        public void Count_With_ZeroValue_ThrowArgumentOfRange()
        {
            var orderItem = new OrderItem(0, 0m, 5);
            Assert.Throws<ArgumentOutOfRangeException>(() => orderItem.Count = 0);

        }
        [Fact]
        public void Count_With_PositiveValue_ThrowArgumentOfRange()
        {
            var orderItem = new OrderItem(0, 0m, 1);
            orderItem.Count = 5;
            Assert.Equal(5, orderItem.Count);

        }
    }
}
