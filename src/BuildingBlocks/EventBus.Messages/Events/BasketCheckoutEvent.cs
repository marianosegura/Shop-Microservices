namespace EventBus.Messages.Events
{
    public class BasketCheckoutEvent : IntegrationBaseEvent
    {  // same as Order entity at Ordering.Domain
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }

        public string FirstName { get; set; }  // billing address data 
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public string CardName { get; set; }  // payment data
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public int PaymentMethod { get; set; }
    }
}
