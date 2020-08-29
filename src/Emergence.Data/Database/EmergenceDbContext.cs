using Emergence.Data.Shared.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Emergence.Data.Repository
{
    public class EmergenceDbContext : DbContext
    {
        private bool IsLoggingEnabled { get; }

        public EmergenceDbContext(DbContextOptions<EmergenceDbContext> options) : base(options)
        {
            IsLoggingEnabled = true;
        }

        public static readonly ILoggerFactory Logger = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information)
                .AddDebug()
                .AddConsole();
        });

        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<InventoryItem> InventoryItems { get; set; }
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<Origin> Origins { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Lifeform> Lifeforms { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<PlantInfo> PlantInfos { get; set; }
        public virtual DbSet<Specimen> Specimens { get; set; }
        public virtual DbSet<Taxon> Taxons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (IsLoggingEnabled)
            {
                options.UseLoggerFactory(Logger)
                .EnableSensitiveDataLogging(true);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>().HasKey(i => i.Id);
            modelBuilder.Entity<InventoryItem>().HasKey(i => i.Id);
            modelBuilder.Entity<Activity>().HasKey(i => i.Id);
            modelBuilder.Entity<Origin>().HasKey(i => i.Id);
            modelBuilder.Entity<Location>().HasKey(i => i.Id);
            modelBuilder.Entity<Lifeform>().HasKey(i => i.Id);
            modelBuilder.Entity<PlantInfo>().HasKey(i => i.Id);
            modelBuilder.Entity<Photo>().HasKey(i => i.Id);
            modelBuilder.Entity<Specimen>().HasKey(i => i.Id);
            modelBuilder.Entity<Taxon>().HasKey(i => i.Id);
        }
    }
}
