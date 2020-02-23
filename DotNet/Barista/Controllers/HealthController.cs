using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Barista.Models;
using Microsoft.Extensions.Logging;
using Barista.Services;

namespace Barista.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly ILogger _logger;

        public HealthController(ILogger<HealthController> logger, OrderProcessorService orderProcessorService)
        {
            _logger = logger;

        }

        // GET api/values
        [HttpGet("status")]
        public ActionResult Get() => Ok();


    }
}
