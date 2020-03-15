using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Http;
using Maker.Interfaces;
using Amqp;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Maker.Interfaces.DTOs;
using System.Text;
using System.Net;
using Consul;

namespace Maker.Services
{

    public class CoffeeProcessor : BackgroundService
    {
        private readonly ILogger<CoffeeProcessor> _logger;
        private readonly IConfiguration _config;
        private readonly Guid nodeId;
        public CoffeeProcessor(
            IConfiguration config,
            ILogger<CoffeeProcessor> logger)
        {
            _logger = logger;
            _config = config;
            nodeId = Guid.NewGuid();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                $"CoffeeProcessor Service is running.{Environment.NewLine}" +
                $"{Environment.NewLine}Tap W to add a work item to the " +
                $"background queue.{Environment.NewLine}");

            await BackgroundProcessing(stoppingToken);
        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                try
                {
                    Connection connection = await Connection.Factory.CreateAsync(await GetAmqpAddressAsync());
                    Amqp.Session session = new Amqp.Session(connection);
                    ReceiverLink receiver = new ReceiverLink(session, "coffeemachine-receiver", "StartNewCup");

                    //receive message
                    Message message = await receiver.ReceiveAsync();
                    if (message == null)
                    {
                        continue;
                    }


                    //handle message
                    _logger.LogInformation(message.ToString());
                    _logger.LogInformation(message.Body.ToString());
                    var requestCup = JsonConvert.DeserializeObject<RequestCup>(message.Body.ToString());
                    Maker.Controllers.MakerController.WorkingOrders.Add(new Order()
                    {
                        Id = requestCup.Id,
                        Coffee = requestCup.Coffee,
                        Started = DateTime.UtcNow
                    });

                    receiver.Accept(message);


                    await receiver.CloseAsync();
                    await session.CloseAsync();
                    await connection.CloseAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                       ex.Message);
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Queued Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }

        private async Task<Address> GetAmqpAddressAsync()
        {
            using (var consulClient = new ConsulClient(config =>
            {
                config.Address = new Uri(_config.GetValue<string>("ConsulURI"));
            }))
            {
                try
                {
                    var baristaKeyNameKV = await consulClient.KV.Get("AMQPMakerKeyName");
                    var baristaKeyName = WebUtility.UrlEncode(Encoding.UTF8.GetString(baristaKeyNameKV.Response.Value));
                    var baristaKeyKV = await consulClient.KV.Get("AMQPMakerKey");
                    var baristaKey = WebUtility.UrlEncode(Encoding.UTF8.GetString(baristaKeyKV.Response.Value));
                    var startNewCupHostKV = await consulClient.KV.Get("AMQPStartNewCupQueue");
                    var startNewCupHost = Encoding.UTF8.GetString(startNewCupHostKV.Response.Value);
                    _logger.LogInformation($"amqps://{baristaKeyName}:{baristaKey}@{startNewCupHost}/");
                    return new Address($"amqps://{baristaKeyName}:{baristaKey}@{startNewCupHost}/");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }

            return null;
        }
    }
}