using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swagger.API;
using Swagger.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Ledger
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            InnerSaceAPI = configuration.GetSection("InnerSaceAPI").Get<InnerSaceAPI>();
            string idpUrl = configuration.GetSection("Authentication:IdentityServerAddress")?.Value;
            APIClient = new APIClient(InnerSaceAPI.Name + "_" + env.EnvironmentName, "secret", idpUrl);
        }
        public InnerSaceAPI InnerSaceAPI { get; set; }
        public APIClient APIClient { get; set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors();
            string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string baseDirectory = AppContext.BaseDirectory;

            services.AddSwagger(APIClient, InnerSaceAPI, Path.Combine(baseDirectory, xmlFile));
            services.AddAuthentication("ID4")

              .AddIdentityServerAuthentication("ID4", options =>
              {
                  options.RequireHttpsMetadata = false;
                  options.ApiName = InnerSaceAPI.Name;
                  options.Authority = APIClient.IdpUrl;
                  options.ClaimsIssuer = APIClient.IdpUrl;
                  options.SupportedTokens = SupportedTokens.Jwt;


              });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } 
            app.UseAuthentication();
            app.UseCors(builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    );
            app.UseSwagger();


            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                c.InjectJavascript("swaggerinit.js");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", InnerSaceAPI.Title);
                c.OAuthClientId(InnerSaceAPI.ClientId);
                c.OAuthAppName(InnerSaceAPI.Name);
            });
            app.UseMvc();
            app.UseStaticFiles();
        }
    }
}
