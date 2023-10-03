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
    public class PurchaseMethodConfiguration : IEntityTypeConfiguration<PurchaseMethod>
    {
        public void Configure(EntityTypeBuilder<PurchaseMethod> builder)
        {
            builder.ToTable("PurchaseMethod");
            builder.HasKey(c => c.PurchaseMethodID);
            builder.Property(c => c.MethodName).HasColumnType("nvarchar(200)");
            builder.Property(c => c.Status).HasColumnType("int");
        }
    }
}
