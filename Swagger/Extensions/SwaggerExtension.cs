using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Swagger.API;
using Swagger.Authorization;
using Swashbuckle.AspNetCore.Swagger;

namespace Swagger.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwagger(
            this IServiceCollection services,
            APIClient client,
            InnerSaceAPI api,
            string filePath
           )
        {

            var scopes = api.Scope.Split(',');
            var scopeDictionary = new Dictionary<string, string>();
            foreach (string scope in scopes)
                scopeDictionary.Add(scope, scope);
            services.AddSwaggerGen(o =>
            {

                o.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = api.AuthorizationFLow,
                    AuthorizationUrl = $"{client.IdpUrl}connect/authorize",
                    TokenUrl = $"{client.IdpUrl}connect/token",
                    Scopes = scopeDictionary


                });


                o.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                    { "oauth2", new  [] { api.Name}},
                });


                o.CustomSchemaIds(x => x.FullName);
                o.SwaggerDoc(api.Version, new Info { Title = api.Title, Version = api.Version });
                o.OperationFilter<SwaggerAuthorizationHeaderParameterOperationFilter>();


                o.IncludeXmlComments(filePath);
            });

            return services;
        }
    }
}
