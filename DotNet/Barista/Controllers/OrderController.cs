using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Logging;
using Barista.Services;
using Barista.Interfaces;
using Barista.Interfaces.DTOs;
using CoffeeMachine.Interfaces;

namespace Barista.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly CoffeeMachine.Interfaces.IClient _coffeeMachineClient;
        private readonly CoffeeMachine.Interfaces.IMessagingClient _messengerClient;
        public OrderController(ILogger<OrderController> logger, IBackgroundTaskQueue taskQueue, CoffeeMachine.Interfaces.IClient coffeeMachineClient, CoffeeMachine.Interfaces.IMessagingClient messengerClient)
        {
            _logger = logger;
            _taskQueue = taskQueue;
            _coffeeMachineClient = coffeeMachineClient;
            _messengerClient = messengerClient;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Order>> Get()
        {
            _logger.LogInformation("orders retrieved");
            return new ActionResult<IEnumerable<Order>>(from o in _taskQueue.QueuedOrders()
                                                        select o);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Order> Get(int id)
        {
            _logger.LogInformation("single order retrieved");
            return new ActionResult<Order>(_taskQueue.QueuedOrders().FirstOrDefault(o=>o.Id == id));
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody] Order order)
        {
            _logger.LogInformation("order added");
            // _taskQueue.QueueBackgroundWorkItem(order, Factories.TaskFactory.CreateMakeCoffeeJob(_logger, _coffeeMachineClient, order, _taskQueue));
            await _messengerClient.StartNewCup(new CoffeeMachine.Interfaces.DTOs.RequestCup()
                    {
                        Id = order.Id,
                        Coffee = order.Coffee
                    });
        }

        //See past orders
        [HttpGet("PastOrders")]
        public async Task<IEnumerable<CoffeeMachine.Interfaces.DTOs.Order>> GetPastOrders() {
            _logger.LogInformation("passed orders retrieved");
            return await _coffeeMachineClient.GetPastOrders();
        }
    }
}
