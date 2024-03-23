using ShoppingCartApi.Models.Domain;

namespace ShoppingCartApi.Models.DTO
{
    public class OrderDTOResponse
    {
            public Guid OrderID { get; set; }
            public Guid UserID { get; set; }
            public DateTime OrderDate { get; set; }
            public IEnumerable<ShoppingCart> Items { get; set; }
            public decimal TotalAmount { get; set; }
    }
}
