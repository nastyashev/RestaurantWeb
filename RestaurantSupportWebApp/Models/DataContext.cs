using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace RestaurantSupportWebApp.Models
{
    public class DataContext : IdentityDbContext<Staff>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        #region DbSets
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuItem_Product> MenuItem_Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Order_MenuItem> Order_MenuItems { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInCart> ProductsInCart { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Table> Tables { get; set; }
        #endregion DbSets
    }
}
