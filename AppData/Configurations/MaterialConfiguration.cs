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
    public class MaterialConfiguration : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.ToTable("Material");
            builder.HasKey(c => c.MaterialId);
            builder.Property(c => c.MaterialCode).HasColumnType("nvarchar(50)");
            builder.Property(c => c.Name).HasColumnType("nvarchar(200)");
            builder.Property(c => c.Status).HasColumnType("int");
            builder.Property(c => c.DateCreated).HasColumnType("Datetime");
        }
    }
}
