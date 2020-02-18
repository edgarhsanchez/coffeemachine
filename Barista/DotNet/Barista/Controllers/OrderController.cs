using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Barista.Models;

namespace Barista.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Order>> Get() => new ActionResult<IEnumerable<Order>>(from o in Barista.Models.Queue.Current
                                                                                              select o.Value);

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Order> Get(int id) => new ActionResult<Order>(Barista.Models.Queue.Current[id]);

        // POST api/values
        [HttpPost]
        public void Post([FromBody] Order value)
        {
            Barista.Models.Queue.Current[value.Id] = value;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Order value)
        {
            Barista.Models.Queue.Current[value.Id] = value;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Order val;
            Barista.Models.Queue.Current.TryRemove(id, out val);
        }
    }
}
