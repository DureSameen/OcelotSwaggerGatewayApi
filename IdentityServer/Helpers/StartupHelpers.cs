using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Configurations;
using IdentityServer.Configurations.ApplicationParts;
using IdentityServer.Data;
using IdentityServer.Helpers.Localization;
using IdentityServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SendGrid;

namespace IdentityServer.Helpers
{
    public static class StartupHelpers
    {

        public static void AddStandardLocalization(this IServiceCollection services)
        {
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            services.AddTransient(typeof(IGenericControllerLocalizer<>), typeof(GenericControllerLocalizer<>));
            services.AddLocalization(options => options.ResourcesPath = ConfigurationConstants.ResourcesPath );
            services.AddControllersWithViews(o => o.Conventions.Add(new GenericControllerRouteConvention())).AddDataAnnotationsLocalization()
                .ConfigureApplicationPartManager(m =>
                {
                    m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider<ApplicationUser, string>());
                })
        .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = ConfigurationConstants.ResourcesPath; });

            services.Configure<RequestLocalizationOptions>(
            opts =>
            {
                var supportedCultures = new[]
                {
                        new CultureInfo("en")
                };

                opts.DefaultRequestCulture = new RequestCulture("en");
                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;
            });
        }
        public static void AddEmailSenders(this IServiceCollection services, IConfiguration configuration)
        {
            var sendgridConnectionString = configuration.GetConnectionString(ConfigurationConstants.SendGridConnectionStringKey);
            var smtpConfiguration = configuration.GetSection(nameof(SmtpConfiguration)).Get<SmtpConfiguration>();
            var sendgridConfiguration = configuration.GetSection(nameof(SendgridConfiguration)).Get<SendgridConfiguration>();

            if (!string.IsNullOrWhiteSpace(sendgridConfiguration.ApiKey))
            {
                // services.AddSingleton<ISendGridClient>(_ => new SendGridClient(sendgridConnectionString));
                services.AddSingleton(sendgridConfiguration);
                services.AddTransient<IEmailSender, SendgridEmailSender>();
            }
            else if (smtpConfiguration != null && !string.IsNullOrWhiteSpace(smtpConfiguration.Host))
            {
                services.AddSingleton(smtpConfiguration);
                services.AddTransient<IEmailSender, SmtpEmailSender>();
            }
            else
            {
                services.AddSingleton<IEmailSender, EmailSender>();
            }
        }

        public static void ConfigureLocalization(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
        }
    }
}
