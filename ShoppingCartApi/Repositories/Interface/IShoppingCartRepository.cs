using ShoppingCartApi.Models.Domain;

namespace ShoppingCartApi.Repositories.Interface
{
    public interface IShoppingCartRepository
    {
        Task<IEnumerable<ShoppingCart>> GetShoppingCartItemsByOrderIdAsync(Guid orderId);
        Task<IEnumerable<ShoppingCart>> GetShoppingCartItemsByShoppingCartIdAsync( Guid shoppingCartItemID);
        Task<ShoppingCart> AddItemToCartAsync(ShoppingCart shoppingCart);
        Task<ShoppingCart> UpdateCartItemQuantityAsync(ShoppingCart shoppingCart);
        Task<ShoppingCart> RemoveItemFromCartAsync(Guid shoppingCartItemID);
    }
}
