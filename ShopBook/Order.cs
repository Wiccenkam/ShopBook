﻿using ShopBook.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopBook
{
    public class Order
    {
        private readonly OrderDto dto;
        public int Id => dto.Id;

        
        public string CellPhone
        {
            get => dto.CellPhone;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(CellPhone));
                dto.CellPhone = value;
            }
        }
            
        public OrderDelivery Delivery 
        {
            get
            {
                if (dto.DeliveryUniqueCode == null)
                    return null;
                return new OrderDelivery(dto.DeliveryUniqueCode, dto.DeliveryDescription, 
                    dto.DeliveryParameters,dto.DeliveryPrice);
            }
            set
            {
                if (value == null)
                    throw new ArgumentException(nameof(Delivery));

                dto.DeliveryUniqueCode = value.UniqueCode;
                dto.DeliveryDescription = value.Description;
                dto.DeliveryParameters = value.Parameters.ToDictionary(p => p.Key, p => p.Value);
                dto.DeliveryPrice = value.Price;
            }
        }
        public OrderPayment Payment
        {
            get
            {
                if (dto.DeliveryUniqueCode == null)
                    return null;
                return new OrderPayment(dto.PaymentServiceName, dto.PaymentDescription,
                    dto.PaymentParameters);
            }
            set
            {
                if (value == null)
                    throw new ArgumentException(nameof(Payment));

                dto.PaymentServiceName = value.UniqueCode;
                dto.PaymentDescription= value.Description;
                dto.PaymentParameters = value.Parameters.ToDictionary(p => p.Key, p => p.Value);
                
            }
        }

        public OrderItemCollection Items { get; }
      
        public int TotalCount => Items.Sum(items => items.Count);
      
        public decimal TotalPrice
        {
            get { return Items.Sum(items => items.Price * items.Count)
                    + (Delivery?.Price??0m); }
        }
        public Order(OrderDto dto)
        {
            
            this.dto = dto;
            Items = new OrderItemCollection(dto);
        }
        public static class DtoFactory 
        {
            public static OrderDto Create() => new OrderDto();
        }

        public static class Mapper
        {
            public static Order Map(OrderDto dto) => new Order(dto);
            public static OrderDto Map(Order domain) => domain.dto;
        }
   
        
    }
}
