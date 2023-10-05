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
    public class ShoesDetails_SizeConfiguration : IEntityTypeConfiguration<ShoesDetails_Size>
    {
        public void Configure(EntityTypeBuilder<ShoesDetails_Size> builder)
        {
            builder.ToTable("ShoesDetails_Size");
            builder.HasKey(c => c.ID);
            builder.Property(c => c.Quantity).HasColumnType("int");
            builder.HasOne(c => c.Size).WithMany(c => c.ShoesDetails_Size).HasForeignKey(c => c.SizeID);
            builder.HasOne(c => c.ShoesDetails).WithMany(c => c.ShoesDetails_Size).HasForeignKey(c => c.ShoesDetailsId);
        }
    }
}
