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
    public class StyleConfiguration : IEntityTypeConfiguration<Style>
    {
        public void Configure(EntityTypeBuilder<Style> builder)
        {
            builder.ToTable("Style");
            builder.HasKey(c => c.StyleID);
            builder.Property(c => c.Name).HasColumnType("nvarchar(100)");
            builder.Property(c => c.Status).HasColumnType("int");
        }
    }

}
