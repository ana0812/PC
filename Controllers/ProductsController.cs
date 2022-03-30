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
    public class ProductsController : ControllerBase
    {

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return ProductsRepo.Products;
        }
        [HttpGet("{id}")]
        public Product Get(int id)
        {
            return ProductsRepo.Products.FirstOrDefault(s => s.Id == id);
        }
        [HttpPost]
        public void Post([FromBody]Product p)
        {
            ProductsRepo.Products.Add(p);
        }
        [HttpDelete]
        public void Delete([FromBody] int id)
        {
            ProductsRepo.Products.RemoveAll(s => s.Id == id);
        }
        [HttpPut]
        public void Put([FromBody] Product p)
        {
            ProductsRepo.Products.RemoveAll(s => s.Id == p.Id);
            ProductsRepo.Products.Add(p);
        }
    }
}
