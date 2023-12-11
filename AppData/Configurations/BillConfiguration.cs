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
    public class BillConfiguration : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.ToTable("Bill");
            builder.HasKey(c => c.BillID);
            builder.Property(c => c.BillCode).HasColumnType("nvarchar(100)");
            builder.Property(c => c.CreateDate).HasColumnType("Datetime");
            builder.Property(c => c.ConfirmationDate).HasColumnType("Datetime");
            builder.Property(c => c.DeliveryDate).HasColumnType("Datetime");
            builder.Property(c => c.SuccessDate).HasColumnType("Datetime");
            builder.Property(c => c.CancelDate).HasColumnType("Datetime");
            builder.Property(c => c.UpdateDate).HasColumnType("Datetime");
            builder.Property(c => c.TotalPrice).HasColumnType("decimal(18, 2)");
            builder.Property(c => c.ShippingCosts).HasColumnType("decimal(18, 2)");
            builder.Property(c => c.TotalPriceAfterDiscount).HasColumnType("decimal(18, 2)");
            builder.Property(c => c.Note).HasColumnType("nvarchar(500)");
            builder.Property(c => c.IsPaid).HasColumnType("bit");
            builder.Property(c => c.Status).HasColumnType("int");
            builder.HasOne(c => c.Employee).WithMany(c => c.Bills).HasForeignKey(c => c.EmployeeID).IsRequired(false);
            builder.HasOne(c => c.Customer).WithMany(c => c.Bills).HasForeignKey(c => c.CustomerID);
            builder.HasOne(c => c.Voucher).WithMany(c => c.Bills).HasForeignKey(c => c.VoucherID).IsRequired(false);
            builder.HasOne(c => c.PurchaseMethod).WithMany(c => c.Bills).HasForeignKey(c => c.PurchaseMethodID);
            builder.HasOne(c => c.Address).WithMany(c => c.Bills).HasForeignKey(c => c.AddressID).IsRequired(false);
        }
    }
}
