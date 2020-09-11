using System.IO;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Repository;
using Emergence.Data.Shared.Stores;
using Emergence.Service;
using Emergence.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Emergence.Transform.Runner
{
    public class Program
    {
        public static IConfiguration Configuration;

        public static async Task Main(string[] args)
        {
            // build config
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables()
                .Build();

            Configuration = configuration;

            // create service collection
            var services = new ServiceCollection();
            ConfigureServices(services, configuration);

            // create service provider
            var serviceProvider = services.BuildServiceProvider();

            // entry to run app
            await serviceProvider.GetService<Runner>().Run(args);
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // configure logging
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });

            services.AddSingleton(configuration);

            // DbContext
            services.AddDbContext<EmergenceDbContext>(options =>
                options.UseSqlServer(
                    configuration["EmergenceDbConnection"]));

            // Application Services
            services.AddTransient<IActivityService, ActivityService>();
            services.AddTransient<IInventoryService, InventoryService>();
            services.AddTransient<ILifeformService, LifeformService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IOriginService, OriginService>();
            services.AddTransient<IPlantInfoService, PlantInfoService>();
            services.AddTransient<ISpecimenService, SpecimenService>();
            services.AddTransient<ITaxonService, TaxonService>();
            services.AddTransient<IBlobService, BlobService>();
            services.AddTransient<IPhotoService, PhotoService>();
            services.AddTransient<IExifService, ExifService>();

            //Add repositories
            services.AddScoped(typeof(IRepository<Activity>), typeof(Repository<Activity>));
            services.AddScoped(typeof(IRepository<Inventory>), typeof(Repository<Inventory>));
            services.AddScoped(typeof(IRepository<InventoryItem>), typeof(Repository<InventoryItem>));
            services.AddScoped(typeof(IRepository<Lifeform>), typeof(Repository<Lifeform>));
            services.AddScoped(typeof(IRepository<Location>), typeof(Repository<Location>));
            services.AddScoped(typeof(IRepository<Origin>), typeof(Repository<Origin>));
            services.AddScoped(typeof(IRepository<Photo>), typeof(Repository<Photo>));
            services.AddScoped(typeof(IRepository<PlantInfo>), typeof(Repository<PlantInfo>));
            services.AddScoped(typeof(IRepository<PlantLocation>), typeof(Repository<PlantLocation>));
            services.AddScoped(typeof(IRepository<Specimen>), typeof(Repository<Specimen>));
            services.AddScoped(typeof(IRepository<Taxon>), typeof(Repository<Taxon>));

            // add app
            services.AddTransient(typeof(IPlantInfoProcessor), typeof(PlantInfoProcessor));
            services.AddTransient<Runner>();
        }
    }
}
