﻿using System;
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
                new OrderItem(1,10m,3),
                new OrderItem(2,100m,5),
                }) ;
            Assert.Equal(3+5, order.TotalCount);
        }
        [Fact]
        public void TotalPrice_WithNonEmptyItems()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,10m,3),
                new OrderItem(2,100m,5),
                });
            Assert.Equal(30m+500m, order.TotalPrice);
        }
        [Fact]
        public void GetItem_WithExistingItem_ReturnItem()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,10m,3),
                new OrderItem(2,100m,5),
                });
            var Orderitem = order.Items.Get(1);
            Assert.Equal(3,Orderitem.Count);
        }
        [Fact]
        public void GetItem_WithNoExistingItem_ReturnItem()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,10m,3),
                new OrderItem(2,100m,5),
                });
            Assert.Throws<InvalidOperationException>(()=> order.Items.Get(100));
            
        }
        [Fact]
        public void AddItem_WithExistingItem_ThrowInvalidOperationException()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,10m,3),
                new OrderItem(2,100m,5),
                });
            var book = new Book(1, null, null, null, null, 0m);
            Assert.Throws<InvalidOperationException>(() =>
            {
                order.Items.Add(1,10m,10);
            });
            order.Items.Add(4, 30m, 10);
            Assert.Equal(3, order.Items.Get(1).Count);

        }

        [Fact]
        public void AddOrUpdateItem_WithNonExistingItem_SetsCount()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,10m,3),
                new OrderItem(2,100m,5),
                });
            
            order.Items.Add(4,30m,10);
            
            Assert.Equal(10, order.Items.Get(4).Count);

        }
        [Fact]
        public void RemoveItem_WithExistingItem_RemoveItem()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,10m,3),
                new OrderItem(2,100m,5),
                });
            order.Items.Remove(1);
            Assert.Collection(order.Items, item => Assert.Equal(2, item.BookId)); 
        }
        [Fact]
        public void RemoveItem_WithNoExistingItem_RemoveItem()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,10m,3),
                new OrderItem(2,100m,5),
                });
            Assert.Throws<InvalidOperationException>(() => order.Items.Remove(100));

        }
    }
}
