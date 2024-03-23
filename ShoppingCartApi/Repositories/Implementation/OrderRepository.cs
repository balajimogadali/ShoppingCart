using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Data;
using ShoppingCartApi.Models.Domain;
using ShoppingCartApi.Repositories.Interface;

namespace ShoppingCartApi.Repositories.Implementation
{
    public class OrderRepository : IOrderRespository
    {
        private readonly ApplicationDbContext dbContext;
        public OrderRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();
            return order;
        }

        public async Task<Order> GetOrderByOrderIDAsync(Guid orderID)
        {
            return await dbContext.Orders.FirstOrDefaultAsync(x => x.OrderID == orderID);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIDAsync(Guid userID)
        {
            return await dbContext.Orders.Where(u=>u.UserID == userID).ToListAsync();
        }

       
    }
}
