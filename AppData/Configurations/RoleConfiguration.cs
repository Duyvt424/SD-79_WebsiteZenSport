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
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.HasKey(c => c.RoleID);
            builder.Property(c => c.RoleCode).HasColumnType("nvarchar(50)");
            builder.Property(c => c.RoleName).HasColumnType("nvarchar(100)");
            builder.Property(c => c.Status).HasColumnType("int");
            builder.Property(c => c.DateCreated).HasColumnType("Datetime");
        }
    }
}
