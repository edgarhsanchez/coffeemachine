using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Barista.Services;
using System.Diagnostics;
using System.Diagnostics.Tracing;

namespace Barista
{
    using System;
    using System.Threading;
    using Amqp.Framing;
    using Amqp.Handler;
    using Amqp.Sasl;
    using Amqp.Types;
    public class Program
    {
        public static void Main(string[] args)
        {

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddLogging();
                    services.AddHostedService<Barista.Services.OrderProcessorService>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder    
                        .UseUrls($"http://0.0.0.0:80")                    
                        .UseStartup<Startup>();
                        
                });
    }
}
