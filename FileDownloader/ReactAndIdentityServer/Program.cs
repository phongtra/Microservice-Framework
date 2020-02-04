using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace ReactAndIdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureGenericHost()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureGenericWebHost()
                    .UseUrls("http://localhost:2100")
                        .UseStartup<Startup>();
                });
    }
}
