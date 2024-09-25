namespace WebApi_CloudNautica.Dto
{
   
    public class OrderResponse
    {
        public Customer Customer { get; set; }
        public Order Order { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class Order
    {
        public int OrderNumber { get; set; }
        public string OrderDate { get; set; }
        public string DeliveryAddress { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public string DeliveryExpected { get; set; }
    }

    public class OrderItem
    {
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal PriceEach { get; set; }
    }
}
