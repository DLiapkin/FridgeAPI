using FridgeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FridgeAPI.Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        DbSet<FridgeModel> FridgeModels { get; set; }
        DbSet<Fridge> Fridges { get; set; }
        DbSet<FridgeProduct> FridgeProducts { get; set; }
        DbSet<Product> Products { get; set; }
    }
}
