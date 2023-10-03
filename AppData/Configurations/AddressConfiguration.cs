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
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");
            builder.HasKey(c => c.AddressID);
            builder.Property(c => c.Street).HasColumnType("nvarchar(200)");
            builder.Property(c => c.Commune).HasColumnType("nvarchar(200)");
            builder.Property(c => c.District).HasColumnType("nvarchar(200)");
            builder.Property(c => c.Province).HasColumnType("nvarchar(200)");
            builder.Property(c => c.Status).HasColumnType("int");
            builder.HasOne(c => c.Customer).WithMany(c => c.Addresses).HasForeignKey(c => c.CumstomerID);
        }
    }
}
