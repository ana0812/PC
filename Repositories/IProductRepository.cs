using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Models;

public interface IProductRepository{
    Task<List<Product>> GetAllProducts();
    Task<Product> GetProduct(string partitionKey,string rowKey);
    Task CreateProduct(Product product);
    Task DeleteProduct(string partitionKey,string rowKey);
}