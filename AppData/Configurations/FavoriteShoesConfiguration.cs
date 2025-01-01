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
    public class FavoriteShoesConfiguration : IEntityTypeConfiguration<FavoriteShoes>
    {
        public void Configure(EntityTypeBuilder<FavoriteShoes> builder)
        {
            builder.ToTable("FavoriteShoes");
            builder.HasKey(c => c.FavoriteShoesID);
            builder.Property(c => c.AddedDate).HasColumnType("Datetime");
            builder.Property(c => c.Status).HasColumnType("int");
            builder.HasOne(c => c.Customer).WithMany(c => c.FavoriteShoes).HasForeignKey(c => c.CumstomerID);
            builder.HasOne(c => c.ShoesDetails_Size).WithMany(c => c.FavoriteShoes).HasForeignKey(c => c.ShoesDetails_SizeId);
        }
    }
}
