using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CoffeeMachine.Interfaces;
using System.Threading.Tasks;
using Amqp;
using System;
using Newtonsoft.Json;
using Amqp.Framing;
using System.Text;
using Consul;
using System.Net;

namespace Barista.ExternalServices {

    public class MessagingClient : CoffeeMachine.Interfaces.IMessagingClient {
        private readonly ILogger<MessagingClient> _logger;
        private readonly IConfiguration _config;

        private readonly Guid nodeId;

        public MessagingClient(ILogger<MessagingClient> logger, IConfiguration config) {
            _logger = logger;
            _config = config;
            nodeId = Guid.NewGuid();
        }

        public async Task StartNewCup(CoffeeMachine.Interfaces.DTOs.RequestCup requestCup) {
            Connection connection = await Connection.Factory.CreateAsync(await GetAmqpAddressAsync());
            Amqp.Session session = new Amqp.Session(connection);
            SenderLink sender = new SenderLink(session, "barista-sender", "StartNewCup");

            //create message
            Message message = new Message(JsonConvert.SerializeObject(requestCup));
            await sender.SendAsync(message);
            await sender.CloseAsync();

            await connection.CloseAsync();
        }

        private async Task<Address> GetAmqpAddressAsync() {            
            using (var consulClient = new ConsulClient(config=>{
                config.Address = new Uri(_config.GetValue<string>("ConsulURI"));
            })) {
                try{
                    var baristaKeyNameKV = await consulClient.KV.Get("AMQPBaristaKeyName");
                    var baristaKeyName = WebUtility.UrlEncode(Encoding.UTF8.GetString(baristaKeyNameKV.Response.Value));
                    var baristaKeyKV = await consulClient.KV.Get("AMQPBaristaKey");
                    var baristaKey = WebUtility.UrlEncode(Encoding.UTF8.GetString(baristaKeyKV.Response.Value));
                    var startNewCupHostKV = await consulClient.KV.Get("AMQPStartNewCupQueue");
                    var startNewCupHost = Encoding.UTF8.GetString(startNewCupHostKV.Response.Value);
                     _logger.LogInformation($"amqps://{baristaKeyName}:{baristaKey}@{startNewCupHost}");
                    return new Address($"amqps://{baristaKeyName}:{baristaKey}@{startNewCupHost}");
                } catch(Exception ex) {
                    _logger.LogError(ex, ex.Message);
                }
            }
           
            return null;
        }
    }
}