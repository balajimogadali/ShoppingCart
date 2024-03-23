namespace ShoppingCartApi.Models.DTO
{
    public class CreateProductDTO
    {
        public Guid ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
