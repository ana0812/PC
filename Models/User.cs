using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Models
{
    public class User : TableEntity
    {
        public User(string name, int id){
            this.PartitionKey = name;
            this.RowKey = id.ToString();
        }

        public User(){}

        public string Email { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

    }
}