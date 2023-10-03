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
    public class CartDetailsConfiguration : IEntityTypeConfiguration<CartDetails>
    {
        public void Configure(EntityTypeBuilder<CartDetails> builder)
        {
            builder.ToTable("CartDetails");
            builder.HasKey(c => c.CartDetailsId);
            builder.Property(c => c.Quantity).HasColumnType("int");
            builder.HasOne(c => c.Cart).WithMany(c => c.CartDetails).HasForeignKey(c => c.CumstomerID);
            builder.HasOne(c => c.ShoesDetails).WithMany(c => c.CartDetails).HasForeignKey(c => c.ShoesDetailsId);
        }
    }
}
