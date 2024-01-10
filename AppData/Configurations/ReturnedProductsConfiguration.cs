using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Configurations
{
	public class ReturnedProductsConfiguration : IEntityTypeConfiguration<ReturnedProducts>
	{
		public void Configure(EntityTypeBuilder<ReturnedProducts> builder)
		{
			builder.ToTable("ReturnedProducts");
			builder.HasKey(c => c.ID);
			builder.Property(c => c.CreateDate).HasColumnType("Datetime");
			builder.Property(c => c.Note).HasColumnType("nvarchar(1000)");
			builder.Property(c => c.QuantityReturned).HasColumnType("int");
			builder.Property(c => c.ReturnedPrice).HasColumnType("decimal(18, 2)");
			builder.Property(c => c.TransactionType).HasColumnType("int");
			builder.Property(c => c.NamePurChaseMethod).HasColumnType("nvarchar(1000)");
			builder.Property(c => c.Status).HasColumnType("int");
			builder.HasOne(c => c.Bill).WithMany(c => c.ReturnedProducts).HasForeignKey(c => c.BillId);
			builder.HasOne(c => c.ShoesDetails_Size).WithMany(c => c.ReturnedProducts).HasForeignKey(c => c.ShoesDetails_SizeID).IsRequired(false);
		}
	}
}
