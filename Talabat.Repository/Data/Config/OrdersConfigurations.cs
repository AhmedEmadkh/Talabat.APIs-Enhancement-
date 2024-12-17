using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Config
{
    internal class OrdersConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(Order => Order.ShippingAddress, shippingAddress => shippingAddress.WithOwner()); // 1 -> 1 mandatory from both sides
            builder.Property(Order => Order.Status)
                .HasConversion(
                    OStatus => OStatus.ToString(),
                    Ostatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), Ostatus)
                );
            builder.Property(Order => Order.Subtotal)
                .HasColumnType("decimal(18,2)");
        }
    }
}
