using System;
using System.Configuration;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Emergence.Client.Server;
using Emergence.Data;
using Emergence.Data.Identity;
using Emergence.Data.Repository;
using Emergence.Data.Shared.Email;
using Emergence.Data.Shared.Stores;
using Emergence.Service;
using Emergence.Service.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Emergence.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration["EmergenceDbConnection"]));
            services.AddDbContext<EmergenceDbContext>(options =>
                options.UseSqlServer(
                    Configuration["EmergenceDbConnection"]));

            // Authentication

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
            services.AddDefaultIdentity<ApplicationUser>(o => o.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var certName = Configuration["CertificateName"];
            if (string.IsNullOrEmpty(certName))
            {
                throw new ConfigurationErrorsException("Unable to read certificate thumbprint");
            }

            var cert = LoadCertificate(certName);
            services.AddIdentityServer()
                .AddSigningCredential(cert)
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
            {
                options.SigningCredential = new SigningCredentials(new X509SecurityKey(cert), "RS256");
            });

            services.AddAuthentication().AddGoogle(options =>
                {
                    options.ClientId = Configuration["GoogleOAuthClientId"];
                    options.ClientSecret = Configuration["GoogleOAuthClientSecret"];
                })
                .AddIdentityServerJwt();

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddLogging();

            // Configuration
            services.AddSingleton<IConfigurationService, ConfigurationService>();

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
            services.AddScoped(typeof(IRepository<Specimen>), typeof(Repository<Specimen>));
            services.AddScoped(typeof(IRepository<Taxon>), typeof(Repository<Taxon>));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Emergence API", Version = "v1" });
            });

            services.AddControllersWithViews();
            services.AddControllers().AddApplicationPart(Assembly.Load("Emergence.API"));
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Emergence V1");
            });

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
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
