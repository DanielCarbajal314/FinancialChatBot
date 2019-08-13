using Financial.Infrastructure.ExternalServices.StooqClient;
using Financial.Infrastructure.MessageQueu;
using Financial.Infrastructure.MessageQueu.BackGroundServices;
using Financial.Infrastructure.MessageQueu.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace Financial.Presentation.BackGoundStockService
{
    class Program
    {
        static IConfiguration config;

        static void Main(string[] args)
        {
            Program.setUpConfiguration();
            var builder = new HostBuilder()
                                .ConfigureServices((services) =>
                                {
                                    Program.ConfigureDI(services);
                                    services.AddHostedService<BackgroundStockQueryService>();
                                });
            builder.RunConsoleAsync();
            Console.ReadKey();
        }


        private static void setUpConfiguration()
        {
            Program.config = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json", true, true)
                                    .Build();
        }

        private static void ConfigureDI(IServiceCollection services)
        {
            services.Configure<RabbitMqConnectionSettings>(Program.config.GetSection("RabbitMqConnectionSettings"));
            services.AddHttpClient<StooqClient>().SetHandlerLifetime(TimeSpan.FromMinutes(5));
            services.AddTransient<IRabbitMessageService, RabbitMessageService>();
            services.AddTransient<IStooqClient, StooqClient>();
            var resolver = services.BuildServiceProvider();
        }
    }
}
