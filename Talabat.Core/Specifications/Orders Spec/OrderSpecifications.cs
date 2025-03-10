﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.Orders_Spec
{
    public class OrderSpecifications : BaseSpecifications<Order>
    {
        // Constructor for getting all orders
        public OrderSpecifications(string buyerEmail)
            : base(O => O.BuyerEmail == buyerEmail)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);

            AddOrderByDesc(O => O.OrderDate);
        }

        // Constructor for getting specific order
        public OrderSpecifications(int orderId, string buyerEmail)
            : base(O => O.Id == orderId && O.BuyerEmail == buyerEmail)
        {
            {
                Includes.Add(O => O.DeliveryMethod);
                Includes.Add(O => O.Items);
            }
        }
    }
}
