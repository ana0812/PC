using Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;
using System.Text;
using Azure.Storage.Queues;

namespace Repositories
{
    public class OrderedProductsRepo : IOrderedProductRepository
    {
        private CloudTableClient _tableOrderedProduct;
        private CloudTable _orderedproductTable;
        private CloudStorageAccount _cloudStorageAccount;
        private string _connectionString;
        private const string queueName = "orderedproductsqueue";
        public OrderedProductsRepo(IConfiguration configuration)
        {
            _connectionString = (string)configuration.GetValue(typeof(string), "AzureStorageAccountConnectionString");
            Task.Run(async () => { await InitializeTable(); })
            .GetAwaiter()
            .GetResult();
        }
        public async Task CreateOrderedProduct(OrderedProduct orderedproduct)
        {
            // var insertOperation =TableOperation.Insert(orderedproduct);
            // await _orderedproductTable.ExecuteAsync(insertOperation);
            QueueClient queueClient = new QueueClient(_connectionString, queueName);

            var message = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(orderedproduct));
            var base64string = System.Convert.ToBase64String(message);

            queueClient.CreateIfNotExists();
            await queueClient.SendMessageAsync(base64string);
        }
        public async Task<List<OrderedProduct>> GetAllOrderedProducts()
        {
            var orderedproduct = new List<OrderedProduct>();
            TableQuery<OrderedProduct> query=new TableQuery<OrderedProduct>();
            TableContinuationToken token =null;
            do{
                TableQuerySegment<OrderedProduct> resultSegment =await _orderedproductTable.ExecuteQuerySegmentedAsync(query,token);
                token= resultSegment.ContinuationToken;
                orderedproduct.AddRange(resultSegment.Results);
            }while(token!=null);
            return orderedproduct;
        }
        public async Task<OrderedProduct> GetOrderedProduct(string partitionKey,string rowKey)
        {
            OrderedProduct orderedproduct=new OrderedProduct();
            TableQuery<OrderedProduct> query=new TableQuery<OrderedProduct>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey",QueryComparisons.Equal,partitionKey),
            TableOperators.And,TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey)));
            TableContinuationToken token =null;
            do{
                TableQuerySegment<OrderedProduct> resultSegment =await _orderedproductTable.ExecuteQuerySegmentedAsync(query,token);
                token= resultSegment.ContinuationToken;
                if(resultSegment.Results.Count!=0)
                {
                orderedproduct=resultSegment.Results[0];
                break;
                }
            }while(token!=null);
            return orderedproduct;
        }
        public async Task DeleteOrderedProduct(string partitionKey,string rowKey)
        {
            OrderedProduct orderedproduct=await GetOrderedProduct(partitionKey,rowKey);
            TableOperation delete= TableOperation.Delete(orderedproduct);
            await _orderedproductTable.ExecuteAsync(delete);
        }
        public async Task InitializeTable()
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);
            _tableOrderedProduct= _cloudStorageAccount.CreateCloudTableClient();
            _orderedproductTable=_tableOrderedProduct.GetTableReference("orderedproduct");

            await _orderedproductTable.CreateIfNotExistsAsync();
        }

    }
}


// new OrderedProduct(){orderID=1,productID=2},
//             new OrderedProduct(){orderID=1,productID=2},
//             new OrderedProduct(){orderID=2,productID=2},
//             new OrderedProduct(){orderID=2,productID=1}