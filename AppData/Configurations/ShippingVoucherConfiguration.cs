using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AppData.Configurations
{
    public class ShippingVoucherConfiguration : IEntityTypeConfiguration<ShippingVoucher>
    {
        public void Configure(EntityTypeBuilder<ShippingVoucher> builder)
        {
            builder.ToTable("ShippingVoucher");
            builder.HasKey(c => c.ShippingVoucherID);
            builder.Property(c => c.VoucherShipCode).HasColumnType("nvarchar(100)");
            builder.Property(c => c.MaxShippingDiscount).HasColumnType("decimal(18, 2)");
			builder.Property(c => c.ShippingDiscount).HasColumnType("decimal(18, 2)");
			builder.Property(c => c.ExpirationDate).HasColumnType("Datetime");
            builder.Property(c => c.QuantityShip).HasColumnType("int");
            builder.Property(c => c.IsShippingVoucher).HasColumnType("int");
            builder.Property(c => c.DateCreated).HasColumnType("Datetime");
        }
    }
}
