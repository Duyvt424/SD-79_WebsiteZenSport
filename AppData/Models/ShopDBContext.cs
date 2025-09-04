using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AppData.Models
{
    public class ShopDBContext : DbContext
    {
        public ShopDBContext()
        {

        }
        public ShopDBContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillDetails> BillDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<FavoriteShoes> FavoriteShoes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PurchaseMethod> PurchaseMethods { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ShoesDetails> ShoesDetails { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Sole> Soles { get; set; }
        public DbSet<Style> Styles { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
		public DbSet<ShippingVoucher> ShippingVoucher { get; set; }
		public DbSet<Material> Materials { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<ShoesDetails_Size> ShoesDetails_Sizes { get; set; }
        public DbSet<BillStatusHistory> BillStatusHistories { get; set; }
        public DbSet<Sex> Sex { get; set; }
        public DbSet<ReturnedProducts> ReturnedProducts { get; set; }
        public DbSet<HistoryChat> HistoryChats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=LAPTOP-46F72MJA\SQLEXPRESS;Initial Catalog=DATN_A;User ID=sa;Password=azx1azx1");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
