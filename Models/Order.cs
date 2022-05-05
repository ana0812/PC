using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Models
{
    public class Order : TableEntity
    {
        public Order(string userID, int orderID){
            this.PartitionKey = userID;
            this.RowKey = orderID.ToString();
        }

        public Order(){}

        public double totalPrice { get; set; }

        public bool PaymentMethod { get; set; }

        public string DeliveryAdress { get; set; }

    }
}