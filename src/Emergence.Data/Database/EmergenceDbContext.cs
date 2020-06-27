using Emergence.Data.Shared.Stores;
using Microsoft.EntityFrameworkCore;

namespace Emergence.Data.Repository
{
    public class EmergenceDbContext : DbContext
    {
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Origin> Origins { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<PlantInfo> PlantInfos { get; set; }
        public DbSet<Specimen> Specimens { get; set; }
        public DbSet<Taxon> Taxons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite("Data Source=emergence.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>().HasKey(i => i.Id);
            modelBuilder.Entity<InventoryItem>().HasKey(i => i.Id);
            modelBuilder.Entity<Activity>().HasKey(i => i.Id);
            modelBuilder.Entity<Origin>().HasKey(i => i.Id);
            modelBuilder.Entity<Location>().HasKey(i => i.Id);
            modelBuilder.Entity<Plant>().HasKey(i => i.Id);
            modelBuilder.Entity<PlantInfo>().HasKey(i => i.Id);
            modelBuilder.Entity<Specimen>().HasKey(i => i.Id);
            modelBuilder.Entity<Taxon>().HasKey(i => i.Id);
        }
    }
}
