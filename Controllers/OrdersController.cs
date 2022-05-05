using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Models;
using Repositories;

namespace L02.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {

        private IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository){
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> Get()
        {
            return await _orderRepository.GetAllOrders();
        }

        [HttpGet("{partitionKey}")]
        public async Task<Order> Get(string partitionKey, string rowKey)
        {
            return await _orderRepository.GetOrder(partitionKey, rowKey);
        }
        [HttpPost]
        public async void Post([FromBody]Order o)
        {
           try{
                    await _orderRepository.CreateOrder(o);
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
                await _orderRepository.DeleteOrder(partitionKey,rowKey);
                }
                catch (System.Exception)
                {
                    throw;
                }
        }
        [HttpPut]
        public async void Put(Order o)
        {
            await _orderRepository.DeleteOrder(o.PartitionKey,o.RowKey);
            await _orderRepository.CreateOrder(o);
        }
    }
}
