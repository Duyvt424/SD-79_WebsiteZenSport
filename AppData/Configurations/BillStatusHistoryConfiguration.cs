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
    public class BillStatusHistoryConfiguration : IEntityTypeConfiguration<BillStatusHistory>
    {
        public void Configure(EntityTypeBuilder<BillStatusHistory> builder)
        {
            builder.ToTable("BillStatusHistory");
            builder.HasKey(c => c.BillStatusHistoryID);
            builder.Property(c => c.Status).HasColumnType("int");
            builder.Property(c => c.ChangeDate).HasColumnType("Datetime");
            builder.Property(c => c.Note).HasColumnType("nvarchar(1000)");
            builder.HasOne(c => c.Bill).WithMany(c => c.BillStatusHistories).HasForeignKey(c => c.BillID);
            builder.HasOne(c => c.Employee).WithMany(c => c.BillStatusHistories).HasForeignKey(c => c.EmployeeID).IsRequired(false);
        }
    }
}
