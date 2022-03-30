using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Repositories;

namespace L02.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {

        [HttpGet]
        public IEnumerable<Order> Get()
        {
            return OrdersRepo.Orders;
        }
        [HttpGet("{id}")]
        public Order Get(int id)
        {
            return OrdersRepo.Orders.FirstOrDefault(s => s.orderID == id);
        }
        [HttpPost]
        public void Post([FromBody]Order o)
        {
            OrdersRepo.Orders.Add(o);
        }
        [HttpDelete]
        public void Delete([FromBody] int id)
        {
            OrdersRepo.Orders.RemoveAll(s => s.orderID == id);
        }
        [HttpPut]
        public void Put([FromBody] Order o)
        {
            OrdersRepo.Orders.RemoveAll(s => s.orderID == o.orderID);
            OrdersRepo.Orders.Add(o);
        }
    }
}
