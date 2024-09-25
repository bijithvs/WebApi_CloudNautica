namespace WebApi_CloudNautica
{


    public class Order
    {
        public int OrderId { get; set; }          // Maps to ORDERID
        public int CustomerId { get; set; }       // Maps to CUSTOMERID
        public DateTime OrderDate { get; set; }   // Maps to ORDERDATE
        public DateTime DeliveryExpected { get; set; } // Maps to DELIVERYEXPECTED
        public bool ContainsGift { get; set; }    // Maps to CONTAINSGIFT
        public List<OrderItem> OrderItems { get; set; }

    }


}

