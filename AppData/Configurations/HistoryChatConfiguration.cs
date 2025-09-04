using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;

namespace AppData.Configurations
{
    public class HistoryChatConfiguration : IEntityTypeConfiguration<HistoryChat>
    {
        public void Configure(EntityTypeBuilder<HistoryChat> builder)
        {
            builder.ToTable("HistoryChat");
            builder.HasKey(c => c.HistoryChatID);
            builder.Property(c => c.Message).HasColumnType("nvarchar(1000)");
            builder.Property(c => c.Response).HasColumnType("nvarchar(1500)");
            builder.Property(c => c.CreatedAt).HasColumnType("Datetime");
            builder.HasOne(c => c.Customer).WithMany(c => c.HistoryChats).HasForeignKey(c => c.CustomerID).IsRequired(false);
        }
    }
}
