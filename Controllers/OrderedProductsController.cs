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
    public class OrderedProductsController : ControllerBase
    {

        [HttpGet]
        public IEnumerable<OrderedProduct> Get()
        {
            return OrderedProductsRepo.OrderedProducts;
        }
        [HttpGet("{id}")]
        public OrderedProduct Get(int id)
        {
            return OrderedProductsRepo.OrderedProducts.FirstOrDefault(s => s.orderID == id);
        }
        [HttpPost]
        public void Post([FromBody]OrderedProduct o)
        {
            OrderedProductsRepo.OrderedProducts.Add(o);
        }
        [HttpDelete]
        public void Delete([FromBody] int id)
        {
            OrderedProductsRepo.OrderedProducts.RemoveAll(s => s.orderID == id);
        }
        [HttpPut]
        public void Put([FromBody] OrderedProduct o)
        {
            OrderedProductsRepo.OrderedProducts.RemoveAll(s => s.orderID == o.orderID);
            OrderedProductsRepo.OrderedProducts.Add(o);
        }
    }
}
