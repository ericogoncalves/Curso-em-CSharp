using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Curso.Core.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.SetBasePath(System.IO.Directory.GetCurrentDirectory())
                                .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",
                                            optional: true, reloadOnChange: true);
                    });

#if DEBUG
                    webBuilder.UseStartup<Startup>();
#endif
#if RELEASE
					webBuilder.UseStartup<Startup>().UseKestrel(x => x.ListenAnyIP());
#endif
                });
    }
}
