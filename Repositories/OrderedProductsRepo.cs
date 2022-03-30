using Models;
using System.Collections.Generic;

namespace Repositories
{
    public static class OrderedProductsRepo
    {
        public static List<OrderedProduct> OrderedProducts = new List<OrderedProduct>()
        {
            new OrderedProduct(){orderID=1,productID=2},
            new OrderedProduct(){orderID=1,productID=2},
            new OrderedProduct(){orderID=2,productID=2},
            new OrderedProduct(){orderID=2,productID=1}
        };
    }
}