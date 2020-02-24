using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Barista.Models;
using Microsoft.Extensions.Logging;
using Barista.Services;
using Barista.Interfaces;
using Barista.Interfaces.DTOs;

namespace Barista.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger _logger;

        public OrderController(ILogger<OrderController> logger, OrderProcessorService orderProcessorService)
        {
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Order>> Get()
        {
            _logger.LogInformation("orders retrieved");
            return new ActionResult<IEnumerable<Order>>(from o in Barista.Models.Queue.Current
                                                        select o.Value);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Order> Get(int id)
        {
            _logger.LogInformation("single order retrieved");
            return new ActionResult<Order>(Barista.Models.Queue.Current[id]);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] Order value)
        {
            _logger.LogInformation("order added");
            Barista.Models.Queue.Current[value.Id] = value;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Order value)
        {
            _logger.LogInformation("order updated");
            Barista.Models.Queue.Current[value.Id] = value;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logger.LogInformation("order deleted");
            Order val;
            Barista.Models.Queue.Current.TryRemove(id, out val);
        }
    }
}
