using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ocelot.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {  // switch ocelot json file depending on environment
                    string environment = hostingContext.HostingEnvironment.EnvironmentName;  // Development or Local
                    config.AddJsonFile($"ocelot.{environment}.json", true, true);  // true optional and reload on change

                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging((hostingContext, loggingBuilder) =>
                {
                    // configure logging using "Logging" section at appsettings
                    loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    loggingBuilder.AddConsole();  // add console and debug logging capabilities
                    loggingBuilder.AddDebug();
                });
    }
}
