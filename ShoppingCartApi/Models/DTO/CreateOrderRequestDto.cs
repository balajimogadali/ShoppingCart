using ShoppingCartApi.Models.Domain;

namespace ShoppingCartApi.Models.DTO
{
    public class CreateOrderRequestDto
    {
        public Guid UserID { get; set; }
        public DateTime OrderDate { get; set; }
      
    }
}
