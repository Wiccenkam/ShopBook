using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ShopBook;

namespace Tests
{
    public class OrderTests
    {
        [Fact]
        public void Order_WithNullItems_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(()=>new Order(1,null));
        }
        [Fact]
        public void TotalCount_WithEmptyItems_ReturnZero()
        {
            var order = new Order(1, new OrderItem[0]);
            Assert.Equal(0,order.TotalCount);
        }
        [Fact]
        public void TotalPrice_WithEmptyItems_ReturnZero()
        {
            var order = new Order(1, new OrderItem[0]);
            Assert.Equal(0m, order.TotalPrice);
        }
        [Fact]
        public void TotalCount_WithNonEmptyItems()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,3,10m),
                new OrderItem(2,5,100m),
                }) ;
            Assert.Equal(3+5, order.TotalCount);
        }
        [Fact]
        public void TotalPrice_WithNonEmptyItems()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,3,10m),
                new OrderItem(2,5,100m),
                });
            Assert.Equal(30m+500m, order.TotalPrice);
        }
        [Fact]
        public void GetItem_WithExistingItem_ReturnItem()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,3,10m),
                new OrderItem(2,5,100m),
                });
            var Orderitem = order.GetItem(1);
            Assert.Equal(3,Orderitem.Count);
        }
        [Fact]
        public void GetItem_WithNoExistingItem_ReturnItem()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,3,10m),
                new OrderItem(2,5,100m),
                });
            Assert.Throws<InvalidOperationException>(()=> order.GetItem(100));
            
        }
        [Fact]
        public void AddOrUpdateItem_WithExistingItem_UpdateCount()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,3,10m),
                new OrderItem(2,5,100m),
                });
            var book = new Book(1, null, null, null, null, 0m);
            order.AddOrUpdateItem(book, 10);
            Assert.Equal(13, order.GetItem(1).Count);

        }

        [Fact]
        public void AddOrUpdateItem_WithNonExistingItem_AddsCount()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,3,10m),
                new OrderItem(2,5,100m),
                });
            var book = new Book(4, null, null, null, null, 0m);
            order.AddOrUpdateItem(book, 10);
            
            Assert.Equal(10, order.GetItem(4).Count);

        }
    }
}
