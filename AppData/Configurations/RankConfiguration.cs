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
    public class RankConfiguration : IEntityTypeConfiguration<Rank>
    {
        public void Configure(EntityTypeBuilder<Rank> builder)
        {
            builder.ToTable("Rank");
            builder.HasKey(c => c.RankID);
            builder.Property(c => c.RankCode).HasColumnType("nvarchar(50)");
            builder.Property(c => c.Name).HasColumnType("nvarchar(100)");
            builder.Property(c => c.Desciption).HasColumnType("nvarchar(500)");
            builder.Property(c => c.ThresholdAmount).HasColumnType("decimal(18, 2)");
            builder.Property(c => c.ReducedValue).HasColumnType("decimal(18, 2)");
            builder.Property(c => c.Status).HasColumnType("int");
            builder.Property(c => c.DateCreated).HasColumnType("Datetime");
        }
    }
}
