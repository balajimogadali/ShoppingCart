using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Data;
using ShoppingCartApi.Models.Domain;
using ShoppingCartApi.Repositories.Interface;

namespace ShoppingCartApi.Repositories.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext dbContext;
        public ProductRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Product> AddProductAsync(Product product)
        {
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product> DeleteProduct(Guid productID)
        {
            var existingCategory = await dbContext.Products.FirstOrDefaultAsync(c => c.ProductID == productID);
            if (existingCategory != null)
            {
                dbContext.Products.Remove(existingCategory);
                await dbContext.SaveChangesAsync();
            }
            return existingCategory;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await dbContext.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIDAsync(Guid productID)
        {
            return await dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == productID);
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var existingCategory = await dbContext.Products.FirstOrDefaultAsync(c => c.ProductID == product.ProductID);
            if (existingCategory != null)
            {
                dbContext.Entry(existingCategory).CurrentValues.SetValues(product);
                await dbContext.SaveChangesAsync();
                return product;
            }
            else
                return null;
        }
    }
}
