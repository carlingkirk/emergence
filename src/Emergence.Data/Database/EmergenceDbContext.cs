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
        public virtual DbSet<PlantLocation> PlantLocations { get; set; }
        public virtual DbSet<Synonym> Synonyms { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserContact> UserContacts { get; set; }
        public virtual DbSet<UserContactRequest> UserContactRequests { get; set; }
        public virtual DbSet<UserMessage> UserMessages { get; set; }
        public virtual DbSet<DisplayName> DisplayNames { get; set; }

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
            modelBuilder.Entity<PlantLocation>().HasKey(i => i.Id);
            modelBuilder.Entity<Synonym>().HasKey(i => i.Id);
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<UserContact>().HasKey(u => u.Id);
            modelBuilder.Entity<UserContactRequest>().HasKey(u => u.Id);
            modelBuilder.Entity<UserMessage>().HasKey(u => u.Id);
            modelBuilder.Entity<DisplayName>().HasNoKey().ToView("DisplayNames").Property(v => v.Name).HasColumnName("Name");

            modelBuilder.Entity<User>().HasIndex(u => u.UserId).IsUnique();
            modelBuilder.Entity<PlantLocation>().HasIndex(pl => new { pl.LocationId, pl.PlantInfoId }).IsUnique();
            modelBuilder.Entity<UserContact>().HasIndex(u => new { u.UserId, u.ContactUserId }).IsUnique();
            modelBuilder.Entity<UserContactRequest>().HasIndex(u => new { u.UserId, u.ContactUserId }).IsUnique();

            modelBuilder.Entity<User>().HasMany(u => u.Contacts).WithOne().HasForeignKey(u => u.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.OthersContacts).WithOne().HasForeignKey(u => u.ContactUserId);

            modelBuilder.Entity<User>().HasMany(u => u.ContactRequests).WithOne().HasForeignKey(u => u.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.OthersContactRequests).WithOne().HasForeignKey(u => u.ContactUserId);

            modelBuilder.Entity<UserContact>().HasOne(uc => uc.User).WithMany(u => u.Contacts);
            modelBuilder.Entity<UserContact>().HasOne(uc => uc.ContactUser).WithMany(u => u.OthersContacts);

            modelBuilder.Entity<UserContactRequest>().HasOne(uc => uc.User).WithMany(u => u.ContactRequests);
            modelBuilder.Entity<UserContactRequest>().HasOne(uc => uc.ContactUser).WithMany(u => u.OthersContactRequests);

            modelBuilder.Entity<UserMessage>().HasOne(u => u.User);
            modelBuilder.Entity<UserMessage>().HasOne(u => u.Sender);

            modelBuilder.Entity<Specimen>().HasOne(s => s.ParentSpecimen);
        }
    }
}
