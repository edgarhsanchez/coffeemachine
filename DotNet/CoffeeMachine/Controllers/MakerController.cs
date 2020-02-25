using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoffeeMachine.Interfaces.DTOs;
using System.Collections.Concurrent;

namespace CoffeeMachine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MakerController : ControllerBase
    {

        private object LockObject = new object();

        private static ConcurrentBag<Order> WorkingOrders = new ConcurrentBag<Order>();

        private readonly ILogger<MakerController> _logger;

        public MakerController(ILogger<MakerController> logger)
        {
            _logger = logger;
        }

        [HttpGet("IsBusy")]
        public bool IsBusy()
        {
            _logger.LogInformation("Busy called");
            var currentOrder = WorkingOrders.FirstOrDefault(order => order.Started.AddMinutes(1).CompareTo(DateTime.UtcNow) > 0);
            return currentOrder != null;
            
        }

        [HttpPost("StartNewCup")]
        public bool StartNewCup([FromBody] RequestCup requestCup)
        {
            _logger.LogInformation("StartNewCup called");
            
            if (IsBusy())
            {
                return false;
            }
            else
            {
                var newOrder = new Order
                {
                    Id = requestCup.Id,
                    Coffee = requestCup.Coffee,
                    Started = DateTime.UtcNow
                };

                WorkingOrders.Add(newOrder);

                return IsBusy();
            }
            
        }

        //See past orders
        [HttpGet("PastOrders")]
        public IEnumerable<Order> GetPastOrders() {
            _logger.LogInformation("passed orders retrieved");
            return WorkingOrders.Where(order =>order.Started.AddMinutes(1).CompareTo(DateTime.UtcNow) < 0 );
        }

    }
}
