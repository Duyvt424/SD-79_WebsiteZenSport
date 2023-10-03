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
    public class ColorConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.ToTable("Color");
            builder.HasKey(c => c.ColorID);
            builder.Property(c => c.Name).HasColumnType("nvarchar(100)");
            builder.Property(c => c.Status).HasColumnType("int");
        }
    }
}
