using Gateway.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Swagger.API;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;

namespace Gateway.Middleware
{
    /// <summary>
    /// Extensions for adding <see cref="SwaggerForOcelotMiddleware"/> into application pipeline.
    /// </summary>
    public static class BuilderExtensions
    {
        /// <summary>
        /// Add Swagger generator for downstream services and UI into application pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>
        /// <see cref="IApplicationBuilder"/>.
        /// </returns>
        public static IApplicationBuilder UseSwaggerForOcelotUI(
           this IApplicationBuilder app,
           IConfiguration configuration, InnerSaceAPI api)
            => app.UseSwaggerForOcelotUI(configuration, null, api);

        /// <summary>
        /// Add Swagger generator for downstream services and UI into application pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="setupAction">Setup <see cref="SwaggerForOCelotUIOptions"/></param>
        /// <returns>
        /// <see cref="IApplicationBuilder"/>.
        /// </returns>
        public static IApplicationBuilder UseSwaggerForOcelotUI(
            this IApplicationBuilder app,
            IConfiguration configuration,
            Action<SwaggerForOCelotUIOptions> setupAction, InnerSaceAPI api)
        {
            var options = new SwaggerForOCelotUIOptions();
            setupAction?.Invoke(options);

            UseSwaggerForOcelot(app, options);

            app.UseSwaggerUI(c =>
            {
                InitUIOption(c, options, api);

                var endPoints = GetConfugration(configuration);
                AddSwaggerEndPoints(c, endPoints, options.EndPointBasePath);
            });

            return app;
        }

        private static void UseSwaggerForOcelot(IApplicationBuilder app, SwaggerForOCelotUIOptions options)
            => app.Map(options.EndPointBasePath, builder => builder.UseMiddleware<SwaggerForOcelotMiddleware>(options));

        private static void AddSwaggerEndPoints(SwaggerUIOptions c, IEnumerable<SwaggerEndPointOptions> endPoints, string basePath)
        {
            foreach (var endPoint in endPoints)
            {
                c.SwaggerEndpoint($"{endPoint.Url}{basePath}", endPoint.Name );
            }
        }

        private static void InitUIOption(SwaggerUIOptions c, SwaggerForOCelotUIOptions options,   InnerSaceAPI api)
        {
            c.ConfigObject = options.ConfigObject;
            c.DocumentTitle = options.DocumentTitle;
            c.HeadContent = options.HeadContent;
            c.IndexStream = options.IndexStream;
            c.OAuthConfigObject = options.OAuthConfigObject;
            c.RoutePrefix = options.RoutePrefix;

            options.InjectJavascript("Swaggerinit.js");
            //c.RoutePrefix = string.Empty;
            //c.SwaggerEndpoint("/swagger/v1/swagger.json", "Messages service getaway API V1");
             c.OAuthClientId(api.ClientId);
             c.OAuthAppName(api.Name);
        }

        private static IEnumerable<SwaggerEndPointOptions> GetConfugration(IConfiguration configuration)
            => configuration.GetSection(SwaggerEndPointOptions.ConfigurationSectionName)
            .Get<IEnumerable<SwaggerEndPointOptions>>();
    }
}
