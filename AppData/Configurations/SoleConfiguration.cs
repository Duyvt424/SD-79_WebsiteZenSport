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
    public class SoleConfiguration : IEntityTypeConfiguration<Sole>
    {
        public void Configure(EntityTypeBuilder<Sole> builder)
        {
            builder.ToTable("Sole");
            builder.HasKey(c => c.SoleID);
            builder.Property(c => c.Name).HasColumnType("nvarchar(100)");
            builder.Property(c => c.Fabric).HasColumnType("nvarchar(100)");
            builder.Property(c => c.Height).HasColumnType("decimal(18, 2)");
            builder.Property(c => c.Status).HasColumnType("int");
        }
    }

}
