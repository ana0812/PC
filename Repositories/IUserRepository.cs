using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Models;

public interface IUserRepository{
    Task<List<User>> GetAllUsers();
    Task<User> GetUser(string partitionKey,string rowKey);
    Task CreateUser(User student);
    Task DeleteUser(string partitionKey,string rowKey);
}