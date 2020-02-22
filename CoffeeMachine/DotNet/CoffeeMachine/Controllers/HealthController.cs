using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using CoffeeMachine.Models;
using Microphone;
using Microsoft.Extensions.Logging;


namespace Barista.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly ILogger _logger;

        public HealthController([FromServices]IClusterClient client, ILogger<HealthController> logger) {
            _logger = logger;
            
        }

        // GET api/values
        [HttpGet("status")]
        public ActionResult Get() => Ok();

        
    }
}
