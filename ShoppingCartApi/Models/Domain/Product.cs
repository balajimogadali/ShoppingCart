namespace ShoppingCartApi.Models.Domain
{
    public class Product
    {
        public Guid ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
