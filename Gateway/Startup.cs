using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Gateway.Extensions;
using Gateway.Middleware;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Swagger.API;
using Swagger.Extensions;

namespace Gateway
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
            services.AddCors(); 
            //Add Swagger
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var baseDirectory = AppContext.BaseDirectory;
            services.AddSwagger(APIClient, InnerSaceAPI, Path.Combine(baseDirectory, xmlFile));
            services.AddSwaggerForOcelot(Configuration);
            services.AddAuthentication("ID4")

           .AddIdentityServerAuthentication("ID4", options =>
           {
               options.RequireHttpsMetadata = false;
               options.ApiName = InnerSaceAPI.Name;
               options.Authority = APIClient.IdpUrl;
               options.ClaimsIssuer = APIClient.IdpUrl;
               options.SupportedTokens = SupportedTokens.Jwt;


           });
            services.AddOcelot(Configuration);
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();
            app.UseAuthentication();
            app.UseCors(builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    );
            app.UseSwaggerForOcelotUI(Configuration, InnerSaceAPI);
            app.UseOcelot()
               .Wait();
          
        }
    }
}
