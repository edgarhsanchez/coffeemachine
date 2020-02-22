using System;
using System.Threading;
using System.Threading.Tasks;
using Microphone;
using Microphone.AspNet;
using Microphone.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Http;

namespace Barista.Services {

    public class OrderProcessorService : IHostedService, IDisposable
    {
        #region Private fields
        private Timer _timer;
        private ILogger<OrderProcessorService> _logger;
        private int _executionCount;
        private IClusterClient _consulClient;
        #endregion

        public OrderProcessorService(ILogger<OrderProcessorService> logger, IClusterProvider mProvider, [FromServices] IClusterClient consulClient ) {
            _consulClient = consulClient;
            _logger = logger;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(CheckCoffeeMachine, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void CheckCoffeeMachine(object state) {
            try{
            var count = Interlocked.Increment(ref _executionCount);

            var uri = _consulClient.GetServiceInstancesAsync("CoffeeMachine").Result.Select(i=>new Uri($"https://{i.Host}:{i.Port}/api/StartNewCup")).First();
            using (var httpClient = new HttpClient()) {
                try{
                    var isBusy = Convert.ToBoolean(httpClient.GetAsync(uri).Result.Content.ReadAsStringAsync().Result);
                }catch (Exception ex) {
                    _logger.LogError(ex.Message);
                }
            }
            
            _logger.LogInformation("CheckCoffeeMachin called from Order Processor: count: " + _executionCount);
            } catch (Exception ex) {
                _logger.LogError(ex.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}