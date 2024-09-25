namespace WebApi_CloudNautica
{

    public class OrderItem
    {
        public int OrderItemId { get; set; }      // Maps to ORDERITEMID
        public int OrderId { get; set; }          // Maps to ORDERID
        public int ProductId { get; set; }        // Maps to PRODUCTID
        public int Quantity { get; set; }         // Maps to QUANTITY
        public decimal Price { get; set; }        // Maps to PRICE
    }

}
