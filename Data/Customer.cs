namespace WebApi_CloudNautica
{
  
    public class Customer
    {
        public int CustomerId { get; set; }      // Maps to CUSTOMERID
        public string FirstName { get; set; }     // Maps to FIRSTNAME
        public string LastName { get; set; }      // Maps to LASTNAME
        public string Email { get; set; }         // Maps to EMAIL
        public string HouseNo { get; set; }       // Maps to HOUSENO
        public string Street { get; set; }        // Maps to STREET
        public string Town { get; set; }          // Maps to TOWN
        public string Postcode { get; set; }      // Maps to POSTCODE
    }


}
