using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Linq;
using Microsoft.Extensions.Logging;
using CoffeeMachine.Interfaces;
using CoffeeMachine.Interfaces.DTOs;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace Barista.ExternalServices
{
    public class CoffeeMachineClient : CoffeeMachine.Interfaces.IClient
    {
        private readonly ILogger<CoffeeMachineClient> _logger;

        public CoffeeMachineClient(ILogger<CoffeeMachineClient> logger)
        {
            _logger = logger;
        }

        public async Task<bool> IsBusy()
        {
            try
            {
                var requestPath = $"{GetHost()}api/maker/IsBusy";
                _logger.LogInformation($"Making request to CoffeeMachine IsBusy at {requestPath}");
                
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await httpClient.GetAsync(requestPath);
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    var isBusy = JsonConvert.DeserializeObject<bool>(content);
                    _logger.LogInformation($"IsBusy: {isBusy}");
                    return isBusy;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<bool> StartNewCup(RequestCup requestCup)
        {
            try
            {
                var requestPath = $"{GetHost()}api/maker/StartNewCup";
                _logger.LogInformation($"Making request to CoffeeMachine StartNewCup at {requestPath}");
                using (var httpClient = new HttpClient())
                {
                    
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var json = JsonConvert.SerializeObject(requestCup);
                    var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(requestPath, stringContent);
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    var startedNewCup = JsonConvert.DeserializeObject<bool>(content);
                    _logger.LogInformation($"StartNewCup: {startedNewCup}");
                    return startedNewCup;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<IEnumerable<Order>> GetPastOrders() {
            try
            {
                var requestPath = $"{GetHost()}api/maker/PastOrders";
                _logger.LogInformation($"Making request to CoffeeMachine StartNewCup at {requestPath}");
                using (var httpClient = new HttpClient())
                {
                    
                
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await httpClient.GetAsync(requestPath);
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    var orders = JsonConvert.DeserializeObject<IEnumerable<Order>>(content);
                    _logger.LogInformation($"GetPastOrders retrieved");
                    return orders;
                
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        private string GetHost()
        {
            return "http://localhost:35000/";
        }

    }
}