﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subtotal)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            Subtotal = subtotal;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDateDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; }
        public DeliveryMethod? DeliveryMethod { get; set; } // Navigational Property [ONE]
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public decimal Subtotal { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;

        public decimal GetTotal()
        {
            if (DeliveryMethod is not null)
                return Subtotal + DeliveryMethod.Cost;
            return 0;
        }
    }
}
