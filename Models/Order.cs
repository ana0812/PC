using System;

namespace Models
{
    public class Order
    {
        public int orderID { get; set; }

        public int userID { get; set; }

        public double totalPrice { get; set; }

        public bool PaymentMethod { get; set; }

        public string DeliveryAdress { get; set; }

    }
}