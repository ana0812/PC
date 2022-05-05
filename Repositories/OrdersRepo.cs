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
    public class OrdersRepo : IOrderRepository
    {
       private CloudTableClient _tableOrder;
        private CloudTable _ordersTable;
        private CloudStorageAccount _cloudStorageAccount;
        private string _connectionString;
        private const string queueName = "ordersqueue";

        public OrdersRepo(IConfiguration configuration)
        {
            _connectionString = (string)configuration.GetValue(typeof(string), "AzureStorageAccountConnectionString");
            Task.Run(async () => { await InitializeTable(); })
            .GetAwaiter()
            .GetResult();
        }
        public async Task CreateOrder(Order order)
        {
            // var insertOperation =TableOperation.Insert(order);
            // await _ordersTable.ExecuteAsync(insertOperation);
            QueueClient queueClient = new QueueClient(_connectionString, queueName);

            var message = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(order));
            var base64string = System.Convert.ToBase64String(message);

            queueClient.CreateIfNotExists();
            await queueClient.SendMessageAsync(base64string);
        }
        public async Task<List<Order>> GetAllOrders()
        {
            var orders = new List<Order>();
            TableQuery<Order> query=new TableQuery<Order>();
            TableContinuationToken token =null;
            do{
                TableQuerySegment<Order> resultSegment =await _ordersTable.ExecuteQuerySegmentedAsync(query,token);
                token= resultSegment.ContinuationToken;
                orders.AddRange(resultSegment.Results);
            }while(token!=null);
            return orders;
        }
        public async Task<Order> GetOrder(string partitionKey,string rowKey)
        {
            Order order=new Order();
            TableQuery<Order> query=new TableQuery<Order>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey",QueryComparisons.Equal,partitionKey),
            TableOperators.And,TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey)));
            TableContinuationToken token =null;
            do{
                TableQuerySegment<Order> resultSegment =await _ordersTable.ExecuteQuerySegmentedAsync(query,token);
                token= resultSegment.ContinuationToken;
                if(resultSegment.Results.Count!=0)
                {
                order=resultSegment.Results[0];
                break;
                }
            }while(token!=null);
            return order;
        }
        public async Task DeleteOrder(string partitionKey,string rowKey)
        {
            Order order=await GetOrder(partitionKey,rowKey);
            TableOperation delete= TableOperation.Delete(order);
            await _ordersTable.ExecuteAsync(delete);
        }
        public async Task InitializeTable()
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);
            _tableOrder= _cloudStorageAccount.CreateCloudTableClient();
            _ordersTable=_tableOrder.GetTableReference("order");

            await _ordersTable.CreateIfNotExistsAsync();
        }

        // public static List<Order> Orders = new List<Order>()
        // {
        //     new Order(){orderID=1,userID=1,totalPrice=30,PaymentMethod=false, DeliveryAdress="aaaaaaa"},
        //     new Order(){orderID=2,userID=2,totalPrice=32,PaymentMethod=true, DeliveryAdress="aaaaaaa"}
        // };
    }
}