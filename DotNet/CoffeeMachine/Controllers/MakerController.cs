﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoffeeMachine.Interfaces.DTOs;

namespace CoffeeMachine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MakerController : ControllerBase
    {
        private static Order _currentOrder = null;

        private readonly ILogger<MakerController> _logger;

        public MakerController(ILogger<MakerController> logger)
        {
            _logger = logger;
        }

        [HttpGet("IsBusy")]
        public ActionResult<bool> IsBusy()
        {
            _logger.LogInformation("Busy called");
            return _currentOrder != null && _currentOrder.Started.AddMinutes(1).CompareTo(DateTime.UtcNow) <= 0;
        }

        [HttpPost("StartNewCup")]
        public ActionResult<bool> StartNewCup([FromBody] RequestCup requestCup)
        {
            _logger.LogInformation("StartNewCup called");
            lock (_currentOrder)
            {
                if (IsBusy().Value)
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

                    _currentOrder = newOrder;

                    return IsBusy().Value;
                }
            }
        }

    }
}
