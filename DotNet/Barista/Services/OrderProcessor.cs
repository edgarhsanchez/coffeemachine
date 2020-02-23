using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Http;
using Consul;

namespace Barista.Services
{

    public class OrderProcessorService : BackgroundService
    {
        #region Private fields
        private ILogger<OrderProcessorService> _logger;
        private CoffeeMachine.Interfaces.IClient _coffeeMachineClient;
        #endregion

        public OrderProcessorService(ILogger<OrderProcessorService> logger, CoffeeMachine.Interfaces.IClient coffeeMachineClient)
        {
            _coffeeMachineClient = coffeeMachineClient;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            try
            {

                _logger.LogInformation("Processing orders...");
                foreach (var orderKeyValue in Barista.Models.Queue.Current)
                {
                    var servicesFound = false;
                    while (!servicesFound)
                    {
                        try
                        {
                            var services = await _coffeeMachineClient.Services();
                            if (services.Count() > 0)
                            {
                                servicesFound = true;

                                foreach (var service in services)
                                {
                                    var isBusy = await _coffeeMachineClient.IsBusy(service);
                                    if (!isBusy)
                                    {
                                        var orderProcessed = await _coffeeMachineClient.StartNewCup(service, new CoffeeMachine.Interfaces.DTOs.RequestCup
                                        {
                                            Id = orderKeyValue.Value.Id,
                                            Coffee = orderKeyValue.Value.Coffee
                                        });
                                        if (!orderProcessed)
                                        {
                                            continue;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        catch
                        {
                            servicesFound = false;
                        }

                        if (!servicesFound)
                        {
                            await Task.Delay(TimeSpan.FromSeconds(1));
                        }
                    }
                }
                _logger.LogInformation("Finished processing orders");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}