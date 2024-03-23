using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.Models.Domain;
using ShoppingCartApi.Models.DTO;
using ShoppingCartApi.Repositories.Interface;

namespace ShoppingCartApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly IProductRepository productRepository;
        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [Authorize(Roles = "Writer")]
        [HttpPost]
        [Route("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO productDTO)
        {
            var product = new Product()
            {
                Name = productDTO.Name,
                Price = productDTO.Price
            };

            await productRepository.AddProductAsync(product);

            var response = new ProductDTO()
            {
                Name = product.Name,
                Price = product.Price,
                ProductID = product.ProductID,
            };

            return Ok(response);
        }

        [Authorize(Roles = "Writer")]
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            var deletedProduct = await productRepository.DeleteProduct(id);

            if (deletedProduct == null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new ProductDTO
            {
                Name = deletedProduct.Name,
                Price = deletedProduct.Price,
                ProductID = deletedProduct.ProductID,
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("{productId:Guid}")]
        public async Task<IActionResult> GetProductById([FromRoute] Guid productId)
        {
            var product = await productRepository.GetProductByIDAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new ProductDTO
            {
                Name = product.Name,
                Price = product.Price,
                ProductID = product.ProductID,
            };

            return Ok(response);
        }

        [Authorize(Roles = "Writer")]
        [HttpPut]
        [Route("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDTO productDTO)
        {
            var product = new Product()
            {
                Name = productDTO.Name,
                Price = productDTO.Price,
                ProductID = productDTO.ProductID,
            };
            await productRepository.UpdateProductAsync(product);

            // Convert Domain model to DTO
            var response = new ProductDTO
            {
                Name = product.Name,
                Price = product.Price,
                ProductID = product.ProductID,
            };

            return Ok(response);

        }

        [HttpGet]
        [Route("GetProductsList")]
        public async Task<ActionResult> GetProducts()
        {
            IEnumerable<Product> response = await productRepository.GetAllProductsAsync();
            var productDto = new List<CreateProductDTO>();

            foreach (var item in response)
            {
                productDto.Add(new CreateProductDTO() { Name= item.Name, Price=item.Price, ProductID=item.ProductID });
            }

            return Ok(productDto);
        }
    }
}
