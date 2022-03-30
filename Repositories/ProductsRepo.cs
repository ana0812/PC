using Models;
using System.Collections.Generic;

namespace Repositories
{
    public static class ProductsRepo
    {
        public static List<Product> Products = new List<Product>()
        {
            new Product(){Id=1,Name="Dark chocolate",Quantity=100,Price=17},
            new Product(){Id=2,Name="Milk chocolate",Quantity=95,Price=15}
        };
    }
}