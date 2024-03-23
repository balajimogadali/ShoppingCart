namespace ShoppingCartApi.Models.DTO
{
    public class UpdateProductDTO
    {
        public Guid ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
