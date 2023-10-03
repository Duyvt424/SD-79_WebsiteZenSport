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
    public class SupplierConfigurationcs : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Supplier");
            builder.HasKey(c => c.SupplierID);
            builder.Property(c => c.Name).HasColumnType("nvarchar(200)");
            builder.Property(c => c.Status).HasColumnType("int");
        }
    }

}
