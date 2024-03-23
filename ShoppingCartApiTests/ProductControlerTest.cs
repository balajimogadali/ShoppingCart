
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppingCartApi.Controllers;
using ShoppingCartApi.Models.Domain;
using ShoppingCartApi.Models.DTO;
using ShoppingCartApi.Repositories.Interface;
using System.Text;

namespace ShoppingCartApiTests
{
    public class ProductControlerTest
    {

        [Fact]
        public async void GetProducts_Returns_OkObjectResult_With_ProductDTOList()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product {  Name="Product1",
                Price=1,
                ProductID=new Guid("300dcb5f-17ed-4759-c942-08dc4ae2799a") },
                
            };
           
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetAllProductsAsync()).ReturnsAsync(products);
            var controller = new ProductController(mockRepository.Object);

            // Act
            var result = await controller.GetProducts();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var productDtoList = Assert.IsAssignableFrom<IEnumerable<CreateProductDTO>>(okResult.Value);
            Assert.Collection(productDtoList,
                item =>
                {
                    Assert.Equal(new Guid("300dcb5f-17ed-4759-c942-08dc4ae2799a"), item.ProductID);
                    Assert.Equal("Product1", item.Name);
                    Assert.Equal(1, item.Price);
                }
                
            );
        }
        [Fact]
        public async void UpdateProduct_Returns_OkObjectResult_With_ProductDTO()
        {
            // Arrange           
          
            var mockRepository = new Mock<IProductRepository>();
      
            var controller = new ProductController(mockRepository.Object);

            UpdateProductDTO updateProductDTO = new UpdateProductDTO() { Name = "Product1", Price = 1, ProductID = new Guid("300dcb5f-17ed-4759-c942-08dc4ae2799a") };

            // Act
            var result = await controller.UpdateProduct(updateProductDTO);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal("Product1", ((ShoppingCartApi.Models.DTO.ProductDTO)((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value).Name);
            Assert.Equal(1, ((ShoppingCartApi.Models.DTO.ProductDTO)((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value).Price);
            Assert.Equal(new Guid("300dcb5f-17ed-4759-c942-08dc4ae2799a"), ((ShoppingCartApi.Models.DTO.ProductDTO)((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value).ProductID);

        }

        [Fact]
        public async Task GetProductById_Returns_OkObjectResult_With_ProductDTO()
        {
            // Arrange
            Guid productId = Guid.NewGuid();
            var mockRepository = new Mock<IProductRepository>();
            var product = new Product { ProductID = productId, Name = "Test Product", Price = 1 };
            mockRepository.Setup(repo => repo.GetProductByIDAsync(productId)).ReturnsAsync(product);
            var controller = new ProductController(mockRepository.Object);

            // Act
            var result = await controller.GetProductById(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var productDto = Assert.IsType<ProductDTO>(okResult.Value);
            Assert.Equal("Test Product", productDto.Name);
            Assert.Equal(1, productDto.Price);
            Assert.Equal(productId, productDto.ProductID);
        }

        [Fact]
        public async Task GetProductById_Returns_NotFoundResult_When_ProductNotFound()
        {
            // Arrange
            Guid productId = Guid.NewGuid();
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetProductByIDAsync(productId)).ReturnsAsync((Product)null);
            var controller = new ProductController(mockRepository.Object);

            // Act
            var result = await controller.GetProductById(productId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_Returns_OkObjectResult_With_DeletedProductDTO()
        {
            // Arrange
            Guid productId = Guid.NewGuid();
            var mockRepository = new Mock<IProductRepository>();
            var deletedProduct = new Product { ProductID = productId, Name = "Test Product", Price = 1 };
            mockRepository.Setup(repo => repo.DeleteProduct(productId)).ReturnsAsync(deletedProduct);
            var controller = new ProductController(mockRepository.Object);

            // Act
            var result = await controller.DeleteProduct(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var productDto = Assert.IsType<ProductDTO>(okResult.Value);
            Assert.Equal("Test Product", productDto.Name);
            Assert.Equal(1, productDto.Price);
            Assert.Equal(productId, productDto.ProductID);
        }

        [Fact]
        public async Task DeleteProduct_Returns_NotFoundResult_When_ProductNotFound()
        {
            // Arrange
            Guid productId = Guid.NewGuid();
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.DeleteProduct(productId)).ReturnsAsync((Product)null);
            var controller = new ProductController(mockRepository.Object);

            // Act
            var result = await controller.DeleteProduct(productId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateProduct_Returns_OkObjectResult_With_ProductDTO()
        {
            // Arrange
            var productDTO = new CreateProductDTO { Name = "Test Product", Price = 1 };
            var product = new Product { Name = productDTO.Name, Price = productDTO.Price };
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.AddProductAsync(It.IsAny<Product>())).ReturnsAsync(product);

            var controller = new ProductController(mockRepository.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(productDTO)));
            controller.HttpContext.Request.Body = memoryStream;
            controller.HttpContext.Request.ContentType = "application/json";

            // Act
            var result = await controller.CreateProduct(productDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var productDto = Assert.IsType<ProductDTO>(okResult.Value);
            Assert.Equal("Test Product", productDto.Name);
            Assert.Equal(1, productDto.Price);
            Assert.NotNull(productDto.ProductID);
        }
    }
}