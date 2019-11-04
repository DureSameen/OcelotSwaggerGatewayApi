using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Ocelot.Middleware;
using Ocelot.DependencyInjection;

namespace Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
             return WebHost.CreateDefaultBuilder(args)
                          .UseStartup<Startup>()
                          .ConfigureAppConfiguration((hostingContext, config) =>
                          {
                              var env = hostingContext.HostingEnvironment;
                              config.AddJsonFile($"ocelot.{env.EnvironmentName}.json")
                              .AddEnvironmentVariables();
                          })
                          
                          .UseIISIntegration()
                          .Build();
        }
    }
}
