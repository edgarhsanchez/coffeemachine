using System;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Linq;
using Microsoft.Extensions.Logging;
using CoffeeMachine.Interfaces;
using CoffeeMachine.Interfaces.DTOs;
using Polly;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace Barista.ExternalServices
{
    public class CoffeeMachineClient : IClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConsulClient _client;
        private readonly ILogger<CoffeeMachineClient> _logger;

        public CoffeeMachineClient(IConfiguration configuration, IConsulClient client, ILogger<CoffeeMachineClient> logger)
        {
            _configuration = configuration;
            _client = client;
            _logger = logger;
        }

        public async Task<bool> IsBusy(Uri uri)
        {
            try
            {
                var requestPath = $"{uri}api/IsBusy";
                _logger.LogInformation($"Making request to CoffeeMachine IsBusy at {requestPath}");
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await httpClient.GetAsync(requestPath).ConfigureAwait(false);
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<bool>(content);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<bool> StartNewCup(Uri uri, RequestCup requestCup)
        {
            try
            {
                var requestPath = $"{uri}api/StartNewCup";
                _logger.LogInformation($"Making request to CoffeeMachine IsBusy at {requestPath}");
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var json = JsonConvert.SerializeObject(requestCup);
                    var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(requestPath, stringContent).ConfigureAwait(false);
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<bool>(content) && response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }


        public async Task<IEnumerable<Uri>> Services()
        {
            var services = await _client.Agent.Services();
            var urls = from service in services.Response
                       where service.Value.Tags.Any(t => t == "Consul") &&
                             service.Value.Tags.Any(t => t == "CoffeeMachine")
                       select new Uri($"{service.Value.Address}:{service.Value.Port}");

            return urls;
        }


    }
}