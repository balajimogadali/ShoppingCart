using ShoppingCartApi.Models.Domain;

namespace ShoppingCartApi.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIDAsync(Guid productID);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<Product> DeleteProduct(Guid productID);
    }
}
