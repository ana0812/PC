using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Models;

public interface IOrderRepository{
    Task<List<Order>> GetAllOrders();
    Task<Order> GetOrder(string partitionKey,string rowKey);
    Task CreateOrder(Order order);
    Task DeleteOrder(string partitionKey,string rowKey);
}