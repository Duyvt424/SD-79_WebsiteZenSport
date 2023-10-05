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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(c => c.ProductID);
            builder.Property(c => c.ProductCode).HasColumnType("nvarchar(50)");
            builder.Property(c => c.Name).HasColumnType("nvarchar(200)");
            builder.Property(c => c.Status).HasColumnType("int");
            builder.Property(c => c.DateCreated).HasColumnType("Datetime");
            builder.HasOne(c => c.Supplier).WithMany(c => c.Products).HasForeignKey(c => c.SupplierID).IsRequired(false);
            builder.HasOne(c => c.Material).WithMany(c => c.Products).HasForeignKey(c => c.MaterialId).IsRequired(false);
        }
    }
}
