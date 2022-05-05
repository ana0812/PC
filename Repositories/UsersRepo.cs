using Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;
using System.Text;
//using Microsoft.Azure.ServiceBus;
using Azure.Storage.Queues;

namespace Repositories
{
    public class UsersRepo : IUserRepository
    {
        private CloudTableClient _tableUser;
        private CloudTable _usersTable;
        private CloudStorageAccount _cloudStorageAccount;
        private string _connectionString;

        private const string queueName = "usersqueue";
        public UsersRepo(IConfiguration configuration)
        {
            _connectionString = (string)configuration.GetValue(typeof(string), "AzureStorageAccountConnectionString");
            Task.Run(async () => { await InitializeTable(); })
            .GetAwaiter()
            .GetResult();
        }
        
        //in controller through post is this method called
        public async Task CreateUser(User user)
        {
            // var insertOperation =TableOperation.Insert(user);
            // await _usersTable.ExecuteAsync(insertOperation);
            QueueClient queueClient = new QueueClient(_connectionString, queueName);

            //serializam user
            var message = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(user));
            var base64string = System.Convert.ToBase64String(message);

            //cream coada daca nu exista
            queueClient.CreateIfNotExists();
            await queueClient.SendMessageAsync(base64string);
        }
        public async Task<List<User>> GetAllUsers()
        {
            var user = new List<User>();
            TableQuery<User> query=new TableQuery<User>();
            TableContinuationToken token =null;
            do{
                TableQuerySegment<User> resultSegment =await _usersTable.ExecuteQuerySegmentedAsync(query,token);
                token= resultSegment.ContinuationToken;
                user.AddRange(resultSegment.Results);
            }while(token!=null);
            return user;
        }
        public async Task<User> GetUser(string partitionKey,string rowKey)
        {
            User user=new User();
            TableQuery<User> query=new TableQuery<User>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey",QueryComparisons.Equal,partitionKey),
            TableOperators.And,TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey)));
            TableContinuationToken token =null;
            do{
                TableQuerySegment<User> resultSegment =await _usersTable.ExecuteQuerySegmentedAsync(query,token);
                token= resultSegment.ContinuationToken;
                if(resultSegment.Results.Count!=0)
                {
                user=resultSegment.Results[0];
                break;
                }
            }while(token!=null);
            return user;
        }
        public async Task DeleteUser(string partitionKey,string rowKey)
        {
            User user=await GetUser(partitionKey,rowKey);
            TableOperation delete= TableOperation.Delete(user);
            await _usersTable.ExecuteAsync(delete);
        }
        public async Task InitializeTable()
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);
            _tableUser= _cloudStorageAccount.CreateCloudTableClient();
            _usersTable=_tableUser.GetTableReference("user");

            await _usersTable.CreateIfNotExistsAsync();
        }
    }
}
        // public static List<User> Users = new List<User>()
        // {
        //     new User(){Id=1,Email="ioana.morariu@gmail.com",Password="12345",Phone="0749385514"},
        //     new User(){Id=2,Email="ana.coporan@gmail.com",Password="LEIA",Phone="0720646776"},
        //     new User(){Id=3,Email="patricia.ruhstrat@gmail.com",Password="Fiat500",Phone="0756099765"},
        //     new User(){Id=4,Email="alexandru.rusmir@gmail.com",Password="FOOD",Phone="0784292935"}
        // };
