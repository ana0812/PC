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

        private IOrderedProductRepository _orderedproductRepository;

        public OrderedProductsController(IOrderedProductRepository orderedproductRepository)
        {
            _orderedproductRepository=orderedproductRepository;
        }

         [HttpGet]
        public async Task<IEnumerable<OrderedProduct>> Get()
        {
            return await _orderedproductRepository.GetAllOrderedProducts();
        }

        [HttpGet("{partitionKey}")]
        public async Task<OrderedProduct> Get(string partitionKey,string rowKey)
        {
            return await _orderedproductRepository.GetOrderedProduct(partitionKey, rowKey);
        }
        
        [HttpPost]
        public async Task Post([FromBody]OrderedProduct orderedproducts)
        {
            try{
                    await _orderedproductRepository.CreateOrderedProduct(orderedproducts);
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
                await _orderedproductRepository.DeleteOrderedProduct(partitionKey,rowKey);
                }
                catch (System.Exception)
                {
                    throw;
                }
        }
        [HttpPut]
        public async void Put(OrderedProduct orderedproduct)
        {
            await _orderedproductRepository.DeleteOrderedProduct(orderedproduct.PartitionKey,orderedproduct.RowKey);
            await _orderedproductRepository.CreateOrderedProduct(orderedproduct);
        }
    }
}