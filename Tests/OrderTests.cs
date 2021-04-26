using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ShopBook;
using ShopBook.Data;

namespace Tests
{
    public class OrderTests
    {
      
        private static Order CreateEmptyTestOrder()
        {
            return new Order(new OrderDto
            {
                Id = 1,
                ItemsDtos = new List<OrderItemDto>()

            }); 
            
        }
        private static Order CreateTestOrder()
        {
            return new Order(new OrderDto
            {
                Id = 1,
                ItemsDtos = new List<OrderItemDto>
                {
                    new OrderItemDto { BookId = 1, Price = 10m, Count = 3 },
                    new OrderItemDto { BookId = 2, Price = 20m, Count = 5 },
                }
            }
            );
        }
        [Fact]
        public void TotalCount_WithEmptyItems_ReturnZero()
        {
            var order = CreateEmptyTestOrder();
            Assert.Equal(0,order.TotalCount);
        }
        [Fact]
        public void TotalPrice_WithEmptyItems_ReturnZero()
        {
            var order = CreateEmptyTestOrder();
            Assert.Equal(0m, order.TotalPrice);
        }
        [Fact]
        public void TotalCount_WithNonEmptyItems()
        {
            var order = CreateTestOrder();
            Assert.Equal(3+5, order.TotalCount);
        }
        [Fact]
        public void TotalPrice_WithNonEmptyItems()
        {
            var order = CreateTestOrder();
            Assert.Equal(30m+100m, order.TotalPrice);
        }
        [Fact]
        public void GetItem_WithExistingItem_ReturnItem()
        {
            var order = CreateTestOrder();
            var Orderitem = order.Items.Get(1);
            Assert.Equal(3,Orderitem.Count);
        }
        [Fact]
        public void GetItem_WithNoExistingItem_ReturnItem()
        {
            var order = CreateTestOrder();
            Assert.Throws<InvalidOperationException>(()=> order.Items.Get(100));
            
        }
        [Fact]
        public void AddItem_WithExistingItem_ThrowInvalidOperationException()
        {
            var order = CreateTestOrder();
            
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
            var order = CreateTestOrder();

            order.Items.Add(4,30m,10);
            
            Assert.Equal(10, order.Items.Get(4).Count);

        }
        [Fact]
        public void RemoveItem_WithExistingItem_RemoveItem()
        {
            var order = CreateTestOrder();
            order.Items.Remove(1);
            Assert.Collection(order.Items, item => Assert.Equal(2, item.BookId)); 
        }
        [Fact]
        public void RemoveItem_WithNoExistingItem_RemoveItem()
        {
            var order = CreateTestOrder();
            Assert.Throws<InvalidOperationException>(() => order.Items.Remove(100));

        }
    }
}
