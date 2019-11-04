using Gateway.Configuration;
using Gateway.Configuration.Transformation;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;


namespace Gateway.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds configuration for for <see cref="SwaggerForOcelotMiddleware"/> into <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddSwaggerForOcelot(this IServiceCollection services, IConfiguration configuration)
            => services
            .AddTransient<ISwaggerJsonTransformer, SwaggerJsonTransformer>()
            .Configure<List<ReRouteOptions>>(options => configuration.GetSection("ReRoutes").Bind(options))
            .Configure<List<SwaggerEndPointOptions>>(options
                => configuration.GetSection(SwaggerEndPointOptions.ConfigurationSectionName).Bind(options))
            .AddHttpClient();

       

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ApiGateway", new Swashbuckle.AspNetCore.Swagger.Info() { Title = "some title", Version = "v1" });
            });
            return services;
        }

        public static IApplicationBuilder AddCustomSwaggerUI(this IApplicationBuilder builder)
        {
            var downApis = new List<string> { "oyw.identity.api", "oyw.product.api" };
            builder
               .UseSwagger()
               .UseSwaggerUI(options =>
               {
                   downApis.ForEach(m =>
                   {
                       options.SwaggerEndpoint($"/{m}/swagger.json", m);
                   });
               });
            return builder;
        }
    }
}
