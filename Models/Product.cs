using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Models
{//eventaul adaugam categorie sa fie pt 
    public class Product : TableEntity
    {
        public Product(string Category, string Name){
            this.PartitionKey = Category;
            this.RowKey = Name;
        }

        public Product(){}

        public int Quantity { get; set; }

        public double Price { get; set; }

        public string PictureURL {get; set; }

    }
}