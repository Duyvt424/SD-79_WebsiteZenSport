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
    public class ShoesDetailsConfiguration : IEntityTypeConfiguration<ShoesDetails>
    {
        public void Configure(EntityTypeBuilder<ShoesDetails> builder)
        {
            builder.ToTable("ShoesDetails");
            builder.HasKey(c => c.ShoesDetailsId);
            builder.Property(c => c.ShoesDetailsCode).HasColumnType("nvarchar(100)");
            builder.Property(c => c.DateCreated).HasColumnType("Datetime");
            builder.Property(c => c.Price).HasColumnType("decimal(18, 2)");
            builder.Property(c => c.ImportPrice).HasColumnType("decimal(18, 2)");
            builder.Property(c => c.Description).HasColumnType("nvarchar(1000)");
            builder.Property(c => c.Status).HasColumnType("int");
            builder.Property(c => c.ImageUrl).HasColumnType("nvarchar(1000)");
            builder.HasOne(c => c.Sole).WithMany(c => c.ShoesDetails).HasForeignKey(c => c.SoleID);
            builder.HasOne(c => c.Style).WithMany(c => c.ShoesDetails).HasForeignKey(c => c.StyleID);
            builder.HasOne(c => c.Product).WithMany(c => c.ShoesDetails).HasForeignKey(c => c.ProductID);
            builder.HasOne(c => c.Color).WithMany(c => c.ShoesDetails).HasForeignKey(c => c.ColorID);
        }
    }
}
