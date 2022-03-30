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
    public class StudentsController : ControllerBase
    {

        [HttpGet]
        public IEnumerable<User> Get()
        {
            return UsersRepo.Users;
        }
        [HttpGet("{id}")]
        public User Get(int id)
        {
            return UsersRepo.Users.FirstOrDefault(s => s.Id == id);
        }
        [HttpPost]
        public void Post([FromBody]User user)
        {
            UsersRepo.Users.Add(user);
        }
        [HttpDelete]
        public void Delete([FromBody] int id)
        {
            UsersRepo.Users.RemoveAll(s => s.Id == id);
        }
        [HttpPut]
        public void Put([FromBody] User user)
        {
            UsersRepo.Users.RemoveAll(s => s.Id == user.Id);
            UsersRepo.Users.Add(user);
        }
    }
}
