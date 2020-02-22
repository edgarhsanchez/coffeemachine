using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microphone.WebApi;
using Microphone.AspNet;
using Microphone.Consul;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeMachine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services => {
                    services.AddLogging();
                    services.AddMicrophone<ConsulProvider>();

                    services.Configure<ConsulOptions>(o => {
                        o.Host = Startup.Host.Value;
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
