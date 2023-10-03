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
    public class BillDetailsConfiguration : IEntityTypeConfiguration<BillDetails>
    
    {
        public void Configure(EntityTypeBuilder<BillDetails> builder)
        {
            builder.ToTable("BillDetails");
            builder.HasKey(c => c.ID);
            builder.Property(c => c.Quantity).HasColumnType("int");
            builder.Property(c => c.Price).HasColumnType("decimal(18, 2)");
            builder.Property(c => c.Status).HasColumnType("int");
            builder.HasOne(c => c.Bill).WithMany(c => c.BillDetails).HasForeignKey(c => c.BillID);
            builder.HasOne(c => c.ShoesDetails).WithMany(c => c.BillDetails).HasForeignKey(c => c.ShoesDetailsId);
        }
    }
}
