using Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Identity.WebAPI
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
                    webBuilder
                        .ConfigureGenericWebHost()
                        .UseStartup<Startup>();
                });
    }
}
