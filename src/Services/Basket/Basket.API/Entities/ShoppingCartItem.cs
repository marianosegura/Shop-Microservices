namespace Catalog.API.Entities
{
    public class ShoppingCartItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }  // copied for performance
        public string Color { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
