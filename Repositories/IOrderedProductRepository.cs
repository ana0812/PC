using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Models;

public interface IOrderedProductRepository{
    Task<List<OrderedProduct>> GetAllOrderedProducts();
    Task<OrderedProduct> GetOrderedProduct(string partitionKey,string rowKey);
    Task CreateOrderedProduct(OrderedProduct orderedproduct);
    Task DeleteOrderedProduct(string partitionKey,string rowKey);
}