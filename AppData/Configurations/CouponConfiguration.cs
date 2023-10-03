using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppData.Configurations
{
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.ToTable("Coupon");
            builder.HasKey(c => c.CouponID);
            builder.Property(c => c.CouponCode).HasColumnType("nvarchar(100)");
            builder.Property(c => c.CouponValue).HasColumnType("decimal(18, 2)");
            builder.Property(c => c.ExpirationDate).HasColumnType("Datetime");
            builder.Property(c => c.MaxUsage).HasColumnType("int");
            builder.Property(c => c.RemainingUsage).HasColumnType("int");
            builder.Property(c => c.Status).HasColumnType("int");
        }
    }
}
