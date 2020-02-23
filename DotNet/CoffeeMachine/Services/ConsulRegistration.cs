using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Dns = System.Net.Dns;
using AddressFamily = System.Net.Sockets.AddressFamily;

namespace CoffeeMachine.Services
{
    public class ConsulRegistration : IHostedService
    {
        private CancellationTokenSource _cts;
        private readonly IConsulClient _consulClient;
        private readonly IOptions<ConsulConfig> _consulConfig;
        private readonly ILogger<ConsulRegistration> _logger;
        private readonly IServer _server;
        private string _registrationID;
        private bool _registered = false;
        public bool IsRegistered { get; set; }

        public ConsulRegistration(
            IConsulClient consulClient,
            IOptions<ConsulConfig> consulConfig,
            ILogger<ConsulRegistration> logger,
            IServer server)
        {
            _server = server;
            _logger = logger;
            _consulConfig = consulConfig;
            _consulClient = consulClient;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!_registered)
            {
                try
                {
                    // Create a linked token so we can trigger cancellation outside of this token's cancellation
                    _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                    var name = Dns.GetHostName(); // get container id
                    var ip = Dns.GetHostEntry(name).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);

                    var uri = new Uri($"http://{ip}:8080");
                    _registrationID = $"{_consulConfig.Value.ServiceID}-{80}";
                    _logger.LogInformation("registration id : " + _registrationID);
                    var registration = new AgentServiceRegistration()
                    {
                        ID = _registrationID,
                        Name = _consulConfig.Value.ServiceName,
                        Address = $"{uri.Scheme}://{uri.Host}",
                        Port = 80,
                        Tags = new[] { "consul", "coffeemachine" },
                        Check = new AgentServiceCheck()
                        {
                            HTTP = $"{uri.Scheme}://{uri.Host}:8080/api/health/status",
                            Timeout = TimeSpan.FromSeconds(3),
                            Interval = TimeSpan.FromSeconds(10)
                        }
                    };

                    _logger.LogInformation("Registering in Consul");
                    await _consulClient.Agent.ServiceDeregister(registration.ID, _cts.Token);
                    await _consulClient.Agent.ServiceRegister(registration, _cts.Token);
                    _logger.LogInformation("Registered Consul");
                    _registered = true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                if (!_registered)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            _logger.LogInformation("Deregistering from Consul");
            try
            {
                await _consulClient.Agent.ServiceDeregister(_registrationID, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Deregisteration failed");
            }
        }
    }
}