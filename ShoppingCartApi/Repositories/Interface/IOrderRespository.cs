using ShoppingCartApi.Models.Domain;
using ShoppingCartApi.Models.Domain;
namespace ShoppingCartApi.Repositories.Interface
{
    public interface IOrderRespository
    {

        Task<Order> CreateOrderAsync(Order order);
        Task<Order> GetOrderByOrderIDAsync(Guid orderID);
        Task<IEnumerable<Order>> GetOrdersByUserIDAsync(Guid userID);
    }
}
