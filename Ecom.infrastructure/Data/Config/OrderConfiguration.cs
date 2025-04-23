using Ecom.Core.Entites.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Orders>
    {
        public void Configure(EntityTypeBuilder<Orders> builder)
        {
            //one to one
            builder.OwnsOne(x => x.shippingAddress,
               n => { n.WithOwner(); });
            //one to many
            builder.HasMany(x => x.orderItems).WithOne().OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.status).HasConversion(p => p.ToString(),
                p => (Status)Enum.Parse(typeof(Status), p));

            builder.Property(m => m.SubTotal).HasColumnType("decimal(18,2)");
        }
    }
}
