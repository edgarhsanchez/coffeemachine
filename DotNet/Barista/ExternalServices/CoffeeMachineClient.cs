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
    public class CoffeeMachineClient : IClient
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

        public async Task<bool> StartNewCup(RequestCup requestCup)
        {
            try
            {
                var requestPath = $"{GetHost()}api/maker/StartNewCup";
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

        private string GetHost()
        {
            return "http://localhost:35000/";
        }

    }
}