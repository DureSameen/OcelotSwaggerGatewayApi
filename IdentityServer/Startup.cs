using System.Globalization;
using System.Reflection;
using IdentityServer.Configurations;
using IdentityServer.Configurations.ApplicationParts;
using IdentityServer.Data;
using IdentityServer.Data.Factory;
using IdentityServer.Data.Services;
using IdentityServer.Helpers;
using IdentityServer.Helpers.Localization;
using IdentityServer4.Models; 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
 

namespace IdentityServer 
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
            services.AddStandardLocalization();
            //services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory<ApplicationUser, IdentityRole>>();
            services.AddScoped<ApplicationUserClaimsPrincipalFactory<ApplicationUser, IdentityRole>>();
            string connectionStringIdentity =
                            Configuration.GetConnectionString("ConnectionStringIdentity");
            string connectionString = Configuration.GetConnectionString("ConnectionStringConfiguration");

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ApplicationDbContext>(builder =>
            builder.UseSqlServer(connectionStringIdentity, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

            services.AddIdentity<ApplicationUser, IdentityRole>().AddRoles<IdentityRole>()
                 .AddEntityFrameworkStores<ApplicationDbContext>() 
                 .AddDefaultTokenProviders();
            
            services.AddEmailSenders(Configuration);
            string idpUrl = Configuration.GetSection("Authentication:IdentityServerAddress")?.Value;
            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.IssuerUri = idpUrl;
            }).AddDeveloperSigningCredential()

               .AddOperationalStore(options =>
               {
                   options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));

                   options.EnableTokenCleanup = true;
                   options.TokenCleanupInterval = 30;
               }).AddConfigurationStore(options =>
                         options.ConfigureDbContext = builder =>
                         builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
               .AddAspNetIdentity<ApplicationUser>()
               .AddProfileService<ProfileService<ApplicationUser>>();


            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.ConfigureLocalization();
            app.UseIdentityServer();
            app.UseRouting();
            app.UseCors(builder =>
               builder
                   .AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   );
            app.UseAuthorization(); 
            ApplicationDbInitializer.SeedUsers(roleManager, userManager);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
