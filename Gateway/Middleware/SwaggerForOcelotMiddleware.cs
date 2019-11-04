using Gateway.Configuration;
using Gateway.Configuration.Transformation;
using Kros.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gateway.Middleware
{
    /// <summary>
    /// Swagger for Ocelot middleware.
    /// This middleware generate swagger documentation from downstream services for SwaggerUI.
    /// </summary>
    public class SwaggerForOcelotMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IOptions<List<ReRouteOptions>> _reRoutes;
        private readonly Lazy<Dictionary<string, SwaggerEndPointOptions>> _swaggerEndPoints;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISwaggerJsonTransformer _transformer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerForOcelotMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next delegate.</param>
        /// <param name="options">The options.</param>
        /// <param name="reRoutes">The Ocelot ReRoutes configuration.</param>
        /// <param name="swaggerEndPoints">The swagger end points.</param>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        public SwaggerForOcelotMiddleware(
            RequestDelegate next,
            SwaggerForOCelotUIOptions options,
            IOptions<List<ReRouteOptions>> reRoutes,
            IOptions<List<SwaggerEndPointOptions>> swaggerEndPoints,
            IHttpClientFactory httpClientFactory,
            ISwaggerJsonTransformer transformer)
        {
            _transformer = Check.NotNull(transformer, nameof(transformer));
            _next = Check.NotNull(next, nameof(next));
            _reRoutes = Check.NotNull(reRoutes, nameof(reRoutes));
            Check.NotNull(swaggerEndPoints, nameof(swaggerEndPoints));
            _httpClientFactory = Check.NotNull(httpClientFactory, nameof(httpClientFactory));

            _swaggerEndPoints = new Lazy<Dictionary<string, SwaggerEndPointOptions>>(()
                => swaggerEndPoints.Value.ToDictionary(p => $"/{p.KeyToPath}", p => p));
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public async Task Invoke(HttpContext context)

        {
            var endPoint = GetEndPoint(context.Request.Path);
            var httpClient = _httpClientFactory.CreateClient();

            // for complete endpoint in configuration.json : like "Url": "http://localhost:500*/swagger/v1/swagger.json"
            var content = await httpClient.GetStringAsync(endPoint.Url);
            content = _transformer.Transform(content, _reRoutes.Value.Where(p => p.SwaggerKey == endPoint.Key));

            // for Docker support: configuration.json : like "Url": "http://{0}:500*/swagger/v1/swagger.json"
            //string host = Dns.GetHostAddresses(new Uri("http://host.docker.internal").Host)?[0]?.ToString() ?? "localhost";
            //var content = await httpClient.GetStringAsync(string.Format(endPoint.Url, host));
            //content = _transformer.Transform(content, _reRoutes.Value.Where(p => p.SwaggerKey == endPoint.Key));

            // for localhost with "Url": "http://{0}:500*/swagger/v1/swagger.json"
            //var content = await httpClient.GetStringAsync(string.Format(endPoint.Url, "localhost"));
            //content = _transformer.Transform(content, _reRoutes.Value.Where(p => p.SwaggerKey == endPoint.Key));

            await context.Response.WriteAsync(content);
        }

        private SwaggerEndPointOptions GetEndPoint(string path)
            => _swaggerEndPoints.Value[path];
    }
}
