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
    public class UsersController : ControllerBase
    {

        private IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository=userRepository;
        }

         [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            return await _userRepository.GetAllUsers();
        }

        [HttpGet("{partitionKey}")]
        public async Task<User> Get(string partitionKey,string rowKey)
        {
            return await _userRepository.GetUser(partitionKey, rowKey);
        }
        [HttpPost]
        public async Task Post([FromBody]User user)
        {
            try{
                    await _userRepository.CreateUser(user);
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
                await _userRepository.DeleteUser(partitionKey,rowKey);
                }
                catch (System.Exception)
                {
                    throw;
                }
        }
        [HttpPut]
        public async void Put(User user)
        {
            await _userRepository.DeleteUser(user.PartitionKey,user.RowKey);
            await _userRepository.CreateUser(user);
        }
    }
}