using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.Core.Entities.Order;

namespace Ecom.Infrastructure.Data.config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Orders>
    {
        public void Configure(EntityTypeBuilder<Orders> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, n => { n.WithOwner(); });

            builder.HasMany(o=>o.OrderItems).WithOne(i=>i.Order)
                .HasForeignKey(i => i.OrderId).OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Status).HasConversion(o => o.ToString(),
                o => (Status)Enum.Parse(typeof(Status), o));


            builder.Property(m => m.SubTotal).HasColumnType("decimal(18,2)");
        }
    }
}
