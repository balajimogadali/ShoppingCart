using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Data;
using ShoppingCartApi.Models.Domain;
using ShoppingCartApi.Repositories.Interface;

namespace ShoppingCartApi.Repositories.Implementation
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext dbContext;
        public ShoppingCartRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ShoppingCart> AddItemToCartAsync(ShoppingCart shoppingCart)
        {
            await dbContext.ShoppingCarts.AddAsync(shoppingCart);
            await dbContext.SaveChangesAsync(); 
            return shoppingCart;
        }

        public async Task<IEnumerable<ShoppingCart>> GetShoppingCartItemsByOrderIdAsync(Guid orderId)
        {
            return await dbContext.ShoppingCarts.Where(s => s.OrderID == orderId).ToListAsync();
        }
        public async Task<IEnumerable<ShoppingCart>> GetShoppingCartItemsByShoppingCartIdAsync(Guid shoppingCartID)
        {
            return await dbContext.ShoppingCarts.Where(s=>s.ShoppingCartID== shoppingCartID).ToListAsync();
        }
        public async Task<ShoppingCart> RemoveItemFromCartAsync(Guid shoppingCartID)
        {
            var shoppingCart = await dbContext.ShoppingCarts.FirstOrDefaultAsync(c => c.ShoppingCartID == shoppingCartID);
            if (shoppingCart != null)
            {
                dbContext.ShoppingCarts.Remove(shoppingCart);
                await dbContext.SaveChangesAsync();
            }
            return shoppingCart;
        }

        public async Task<ShoppingCart> UpdateCartItemQuantityAsync(ShoppingCart shoppingCart)
        {
            var exisingShoppingCartItem = await dbContext.ShoppingCarts.FirstOrDefaultAsync(c => c.ShoppingCartID == shoppingCart.ShoppingCartID);
            if (shoppingCart != null)
            {
                dbContext.Entry(exisingShoppingCartItem).CurrentValues.SetValues(shoppingCart);
                await dbContext.SaveChangesAsync();
                return shoppingCart;
            }
            else
                return null;
        }
    }
}
