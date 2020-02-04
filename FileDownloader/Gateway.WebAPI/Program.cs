using System.IO;
using Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Gateway.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureGenericHost()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .AddJsonFile("ocelot.json");
                });
            return builder
                .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder
                            .ConfigureGenericWebHost()
                            .UseStartup<Startup>();
                    }
                );
        }

        public static IHostBuilder CreateHostBuilder_DEL(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",
                            true, true)
                        .AddJsonFile("ocelot.json")
                        .AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, loggingBuilder) =>
                {
                    loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    loggingBuilder.AddConsole();
                    loggingBuilder.AddDebug();
                })
                .UseSerilog((builderContext, config) =>
                {
                    config
                        .MinimumLevel.Information()
                        .Enrich.FromLogContext()
                        .WriteTo.Console();
                });
            return builder
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureKestrel((context, serverOptions) =>
                        {
                            var kestrelSection = context.Configuration.GetSection("Kestrel");
                            serverOptions.Configure(kestrelSection);
//                            serverOptions.Listen(IPAddress.Loopback, 50000);
//                            serverOptions.Listen(IPAddress.Loopback, 50001, 
//                                listenOptions =>
//                                {
//                                    listenOptions.UseHttps("testCert.pfx", 
//                                        "testPassword");
//                                });
                        })
                        .UseStartup<Startup>()
                        ;
                });
        }
    }
}
