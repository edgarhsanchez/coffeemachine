using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoffeeMachine.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MakerController : ControllerBase
    {
        
        private static Lazy<CoffeeMachine.Models.Order> _currentOrder = null;

        public CoffeeMachine.Models.Order CurrentOrder {
            get {
                if(_currentOrder ==null) {
                    _currentOrder = new Lazy<Models.Order>();
                }

                return _currentOrder.Value;
            }
        }
        private readonly ILogger<MakerController> _logger;

        public MakerController(ILogger<MakerController> logger)
        {
            _logger = logger;
        }

        [HttpGet("api/IsBusy")]
        public ActionResult<bool> IsBusy()
        {
            return CurrentOrder != null;
        }



    }
}
