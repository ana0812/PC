using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Models
{
    public class OrderedProduct : TableEntity
    {
         public OrderedProduct(int orderID, int Id)
        {
            this.PartitionKey=orderID.ToString();
            this.RowKey= Id.ToString() ;
        }

        public OrderedProduct(){}

        public int productID { get; set; }

    }
}