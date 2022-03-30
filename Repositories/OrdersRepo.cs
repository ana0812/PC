using Models;
using System.Collections.Generic;

namespace Repositories
{
    public static class OrdersRepo
    {
        public static List<Order> Orders = new List<Order>()
        {
            new Order(){orderID=1,userID=1,totalPrice=30,PaymentMethod=false, DeliveryAdress="aaaaaaa"},
            new Order(){orderID=2,userID=2,totalPrice=32,PaymentMethod=true, DeliveryAdress="aaaaaaa"}
        };
    }
}