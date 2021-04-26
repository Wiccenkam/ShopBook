using ShopBook;
using ShopBook.Data;
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
                OrderItem.DtoFactory.Create(new OrderDto(), 1, 10m, count);
            });
        }
       
        [Fact]
        public void OrderItem_WithNEgativeCount_ThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = -1;
                OrderItem.DtoFactory.Create(new OrderDto(), 1, 10m, count);
            });
        }
        [Fact]
        public void OrderItem_WithPositiveCount_SetCount()
        {
            var orderItem = OrderItem.DtoFactory.Create(new OrderDto(), 1, 3m, 2);
            Assert.Equal(1,orderItem.BookId);
            Assert.Equal(3m, orderItem.Price);
            Assert.Equal(2, orderItem.Count);
            
            
        }
        [Fact]
        public void Count_With_NegativeValue_ThrowArgumentOfRange()
        {
            var orderItemDto = OrderItem.DtoFactory.Create(new OrderDto(), 1, 10m, 1);
            var orderItem = OrderItem.Mapper.Map(orderItemDto);
            Assert.Throws<ArgumentOutOfRangeException>(() => orderItem.Count = -1);

        }
        [Fact]
        public void Count_With_ZeroValue_ThrowArgumentOfRange()
        {
            var orderItemDto = OrderItem.DtoFactory.Create(new OrderDto(), 1, 10m, 1);
            var orderItem = OrderItem.Mapper.Map(orderItemDto);
            Assert.Throws<ArgumentOutOfRangeException>(() => orderItem.Count = 0);

        }
        [Fact]
        public void Count_With_PositiveValue_ThrowArgumentOfRange()
        {
            var orderItemDto = OrderItem.DtoFactory.Create(new OrderDto(), 1, 10m, 1);
            var orderItem = OrderItem.Mapper.Map(orderItemDto);
            orderItem.Count = 5;
            Assert.Equal(5, orderItem.Count);

        }
    }
}
