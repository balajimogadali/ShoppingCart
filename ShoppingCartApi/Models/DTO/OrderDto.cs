namespace ShoppingCartApi.Models.DTO
{
    public class OrderDto
    {
        public Guid OrderID { get; set; }
        public Guid UserID { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
