using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Http;
using CoffeeMachine.Interfaces;

namespace Barista.Services
{

    public class OrderProcessorService : BackgroundService
    {
         private readonly ILogger<OrderProcessorService> _logger;

    public OrderProcessorService(IBackgroundTaskQueue taskQueue, 
        ILogger<OrderProcessorService> logger)
    {
        TaskQueue = taskQueue;
        _logger = logger;
    }

    public IBackgroundTaskQueue TaskQueue { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            $"Queued Hosted Service is running.{Environment.NewLine}" +
            $"{Environment.NewLine}Tap W to add a work item to the " +
            $"background queue.{Environment.NewLine}");

        await BackgroundProcessing(stoppingToken);
    }

    private async Task BackgroundProcessing(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = 
                await TaskQueue.DequeueAsync(stoppingToken);

            try
            {
                await workItem.Item1(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Error occurred executing {WorkItem}.", nameof(workItem));
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Queued Hosted Service is stopping.");

        await base.StopAsync(stoppingToken);
    }
    }
}