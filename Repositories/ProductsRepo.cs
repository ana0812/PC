using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Azure.Storage.Queues;

namespace Repositories
{
    public class ProductsRepo : IProductRepository
    {
        // public static List<Product> Products = new List<Product>()
        // {
        //     new Product(){Id=1,Name="Dark chocolate",Quantity=100,Price=17},
        //     new Product(){Id=2,Name="Milk chocolate",Quantity=95,Price=15}
        // };

        private CloudTableClient _tableProduct;
        private CloudTable _productsTable;
        private CloudStorageAccount _cloudStorageAccount;
        private const string queueName = "productsqueue";
        private string _connectionString;

        public ProductsRepo(IConfiguration configuration)
        {
            _connectionString = (string)configuration.GetValue(typeof(string), "AzureStorageAccountConnectionString");
            Task.Run(async () => { await InitializeTable(); })
            .GetAwaiter()
            .GetResult();
        }

        public async Task CreateProduct(Product product)
        {
            // var insertOperation = TableOperation.Insert(product);
            // await _productsTable.ExecuteAsync(insertOperation);
            QueueClient queueClient = new QueueClient(_connectionString, queueName);

            var message = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(product));
            var base64string = System.Convert.ToBase64String(message);

            queueClient.CreateIfNotExists();
            await queueClient.SendMessageAsync(base64string);
        }

        public async Task DeleteProduct(string partitionKey, string rowKey)
        {
            Product product = await GetProduct(partitionKey,rowKey);
            TableOperation delete = TableOperation.Delete(product);
            await _productsTable.ExecuteAsync(delete);
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var products = new List<Product>();
            TableQuery<Product> query = new TableQuery<Product>();
            TableContinuationToken token = null;

            do{
                TableQuerySegment<Product> resultSegment = await _productsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
                products.AddRange(resultSegment.Results);
            }while(token!=null);

            return products;
        }

        public async Task<Product> GetProduct(string partitionKey, string rowKey)
        {
            Product product = new Product();
            TableQuery<Product> query = new TableQuery<Product>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
            TableOperators.And,TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey)));

            TableContinuationToken token =null;
            do{
                TableQuerySegment<Product> resultSegment =await _productsTable.ExecuteQuerySegmentedAsync(query,token);
                token= resultSegment.ContinuationToken;
                if(resultSegment.Results.Count!=0)
                {
                product=resultSegment.Results[0];
                break;
                }
            }while(token!=null);
            return product;
        }

        public async Task InitializeTable()
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);
            _tableProduct= _cloudStorageAccount.CreateCloudTableClient();
            _productsTable=_tableProduct.GetTableReference("product");

            await _productsTable.CreateIfNotExistsAsync();
        }
    }
}