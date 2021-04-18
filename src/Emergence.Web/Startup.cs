using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using Emergence.Data;
using Emergence.Data.Identity;
using Emergence.Data.Repository;
using Emergence.Data.Shared.Email;
using Emergence.Data.Shared.Stores;
using Emergence.Extensions;
using Emergence.Models;
using Emergence.Service;
using Emergence.Service.Interfaces;
using Emergence.Service.Search;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SearchModels = Emergence.Data.Shared.Search.Models;

namespace Emergence
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.AddSqlConnection(Configuration["EmergenceDbConnection"], typeof(ApplicationDbContext).Assembly.FullName));
            services.AddDbContext<EmergenceDbContext>(options =>
                options.AddSqlConnection(Configuration["EmergenceDbConnection"], typeof(EmergenceDbContext).Assembly.FullName));

            // Authentication
            services.AddDefaultIdentity<ApplicationUser>(o => o.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
            });

            var certName = Configuration["CertificateName"];
            if (string.IsNullOrEmpty(certName))
            {
                throw new ConfigurationErrorsException("Unable to read certificate thumbprint");
            }

            var cert = LoadCertificate(certName);
            services.AddIdentityServer(opt =>
            {
                // There's gotta be some bug around this, we shouldn't have to set this
                opt.IssuerUri = Configuration["IssuerUri"];
            })
            .AddSigningCredential(cert)
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
            {
                options.SigningCredential = new SigningCredentials(new X509SecurityKey(cert), "RS256");
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration["GoogleOAuthClientId"];
                    options.ClientSecret = Configuration["GoogleOAuthClientSecret"];
                })
                .AddIdentityServerJwt().AddCookie();

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddLogging();

            // Configuration
            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddDistributedMemoryCache();

            // Application Services
            services.AddTransient<IActivityService, ActivityService>();
            services.AddTransient<IBlobService, BlobService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IExifService, ExifService>();
            services.AddTransient<IInventoryService, InventoryService>();
            services.AddTransient<ILifeformService, LifeformService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IOriginService, OriginService>();
            services.AddTransient<IPhotoService, PhotoService>();
            services.AddTransient<IPlantInfoService, PlantInfoService>();
            services.AddTransient<ISpecimenService, SpecimenService>();
            services.AddTransient<ISynonymService, SynonymService>();
            services.AddTransient<ITaxonService, TaxonService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserContactService, UserContactService>();
            services.AddTransient<IMessageService, MessageService>();

            // Add repositories
            services.AddScoped(typeof(IRepository<Activity>), typeof(Repository<Activity>));
            services.AddScoped(typeof(IRepository<DisplayName>), typeof(Repository<DisplayName>));
            services.AddScoped(typeof(IRepository<Inventory>), typeof(Repository<Inventory>));
            services.AddScoped(typeof(IRepository<InventoryItem>), typeof(Repository<InventoryItem>));
            services.AddScoped(typeof(IRepository<Lifeform>), typeof(Repository<Lifeform>));
            services.AddScoped(typeof(IRepository<Location>), typeof(Repository<Location>));
            services.AddScoped(typeof(IRepository<Origin>), typeof(Repository<Origin>));
            services.AddScoped(typeof(IRepository<Photo>), typeof(Repository<Photo>));
            services.AddScoped(typeof(IRepository<PlantInfo>), typeof(Repository<PlantInfo>));
            services.AddScoped(typeof(IRepository<PlantLocation>), typeof(Repository<PlantLocation>));
            services.AddScoped(typeof(IRepository<PlantSynonym>), typeof(Repository<PlantSynonym>));
            services.AddScoped(typeof(IRepository<Specimen>), typeof(Repository<Specimen>));
            services.AddScoped(typeof(IRepository<Synonym>), typeof(Repository<Synonym>));
            services.AddScoped(typeof(IRepository<Taxon>), typeof(Repository<Taxon>));
            services.AddScoped(typeof(IRepository<User>), typeof(Repository<User>));
            services.AddScoped(typeof(IRepository<UserContact>), typeof(Repository<UserContact>));
            services.AddScoped(typeof(IRepository<UserContactRequest>), typeof(Repository<UserContactRequest>));
            services.AddScoped(typeof(IRepository<UserMessage>), typeof(Repository<UserMessage>));

            // Add search
            services.AddTransient<ISearchClient<SearchModels.PlantInfo>, SearchClient<SearchModels.PlantInfo>>();
            services.AddTransient<ISearchClient<SearchModels.Specimen>, SearchClient<SearchModels.Specimen>>();
            services.AddTransient<IIndex<SearchModels.PlantInfo, Data.Shared.Models.PlantInfo>, PlantInfoIndex>();
            services.AddTransient<IIndex<SearchModels.Lifeform, Data.Shared.Models.Lifeform>, PlantInfoIndex>();
            services.AddTransient<IIndex<SearchModels.Specimen, Data.Shared.Models.Specimen>, SpecimenIndex>();

            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Emergence API", Version = "v1" });
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddProgressiveWebApp();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        private X509Certificate2 LoadCertificate(string certName)
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(
                    azureServiceTokenProvider.KeyVaultTokenCallback));
            var certSecret = keyVaultClient.GetSecretAsync(Configuration["App:KeyVault"], certName).Result;
            var certBytes = Convert.FromBase64String(certSecret.Value);

            return new X509Certificate2(certBytes);
        }
    }
}