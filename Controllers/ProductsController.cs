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

        private IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository=productRepository;
        }

         [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _productRepository.GetAllProducts();
        }

        [HttpGet("{partitionKey}")]
        public async Task<Product> Get(string partitionKey,string rowKey)
        {
            return await _productRepository.GetProduct(partitionKey, rowKey);
        }
        
        [HttpPost]
        public async Task Post([FromBody]Product product)
        {
            try{
                    await _productRepository.CreateProduct(product);
                }
                catch (System.Exception)
                {
                    throw;
                }
        }
        [HttpDelete]
        public async void Delete(string partitionKey,string rowKey)
        {
                try{
                await _productRepository.DeleteProduct(partitionKey,rowKey);
                }
                catch (System.Exception)
                {
                    throw;
                }
        }
        [HttpPut]
        public async void Put(Product product)
        {
            await _productRepository.DeleteProduct(product.PartitionKey,product.RowKey);
            await _productRepository.CreateProduct(product);
        }
    }
}