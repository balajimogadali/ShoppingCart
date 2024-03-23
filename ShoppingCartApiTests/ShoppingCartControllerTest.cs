using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppingCartApi.Controllers;
using ShoppingCartApi.Models.Domain;
using ShoppingCartApi.Models.DTO;
using ShoppingCartApi.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartApiTests
{
    public class ShoppingCartControllerTest
    {
        [Fact]
        public async Task CreateShoppingCart_Returns_OkObjectResult_With_ShoppingCartDto()
        {
            // Arrange
            var shoppingCartDto = new ShoppingCartDto { OrderID = new Guid(), ProductID = new Guid(), Quantity = 2 };
            var shoppingCart = new ShoppingCart { OrderID = shoppingCartDto.OrderID, ProductID = shoppingCartDto.ProductID, Quantity = shoppingCartDto.Quantity, ShoppingCartID = new Guid() };
            var mockRepository = new Mock<IShoppingCartRepository>();
            mockRepository.Setup(repo => repo.AddItemToCartAsync(It.IsAny<ShoppingCart>())).ReturnsAsync(shoppingCart);
            var controller = new ShoppingCartController(mockRepository.Object);

            // Act
            var result = await controller.CreateShoppingCart(shoppingCartDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ShoppingCartDto>(okResult.Value);
            Assert.Equal(shoppingCartDto.OrderID, response.OrderID);
            Assert.Equal(shoppingCartDto.ProductID, response.ProductID);
            Assert.Equal(shoppingCartDto.Quantity, response.Quantity);
            Assert.Equal(new Guid(), response.ShoppingCartID);
        }

        [Fact]
        public async Task UpdateShoppingCartQuantity_Returns_OkObjectResult_With_ShoppingCartDto()
        {
            // Arrange
            var shoppingCartDto = new ShoppingCartDto { ShoppingCartID = new Guid(), OrderID = new Guid(), ProductID = new Guid(), Quantity = 2 };
            var shoppingCart = new ShoppingCart { ShoppingCartID = shoppingCartDto.ShoppingCartID, OrderID = shoppingCartDto.OrderID, ProductID = shoppingCartDto.ProductID, Quantity = shoppingCartDto.Quantity };
            var mockRepository = new Mock<IShoppingCartRepository>();
            mockRepository.Setup(repo => repo.UpdateCartItemQuantityAsync(It.IsAny<ShoppingCart>())).ReturnsAsync(shoppingCart);
            var controller = new ShoppingCartController(mockRepository.Object);

            // Act
            var result = await controller.UpdateShoppingCartQuantity(shoppingCartDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ShoppingCartDto>(okResult.Value);
            Assert.Equal(shoppingCartDto.ShoppingCartID, response.ShoppingCartID);
            Assert.Equal(shoppingCartDto.OrderID, response.OrderID);
            Assert.Equal(shoppingCartDto.ProductID, response.ProductID);
            Assert.Equal(shoppingCartDto.Quantity, response.Quantity);
        }

        [Fact]
        public async Task DeleteShoppingCartById_Returns_OkObjectResult_With_ShoppingCartDto()
        {
            // Arrange
            var shoppingCartId = Guid.NewGuid();
            var shoppingCart = new ShoppingCart { ShoppingCartID = shoppingCartId, OrderID = new Guid(), ProductID = new Guid(), Quantity = 2 };
            var mockRepository = new Mock<IShoppingCartRepository>();
            mockRepository.Setup(repo => repo.RemoveItemFromCartAsync(shoppingCartId)).ReturnsAsync(shoppingCart);
            var controller = new ShoppingCartController(mockRepository.Object);

            // Act
            var result = await controller.DeleteShoppingCartById(shoppingCartId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ShoppingCartDto>(okResult.Value);
            Assert.Equal(shoppingCart.OrderID, response.OrderID);
            Assert.Equal(shoppingCart.ProductID, response.ProductID);
            Assert.Equal(shoppingCart.Quantity, response.Quantity);
            Assert.Equal(shoppingCart.ShoppingCartID, response.ShoppingCartID);
        }

        [Fact]
        public async Task DeleteShoppingCartById_Returns_NotFoundResult_When_ShoppingCartNotFound()
        {
            // Arrange
            var shoppingCartId = Guid.NewGuid();
            var mockRepository = new Mock<IShoppingCartRepository>();
            mockRepository.Setup(repo => repo.RemoveItemFromCartAsync(shoppingCartId)).ReturnsAsync((ShoppingCart)null);
            var controller = new ShoppingCartController(mockRepository.Object);

            // Act
            var result = await controller.DeleteShoppingCartById(shoppingCartId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetShoppingCartItemsById_Returns_OkObjectResult_With_ShoppingCartDtoList()
        {
            // Arrange
            var shoppingCartId = Guid.NewGuid();
            var shoppingCartItems = new List<ShoppingCart>
            {
                new ShoppingCart { ShoppingCartID = shoppingCartId, OrderID = new Guid(), ProductID = new Guid(), Quantity = 2 },
                new ShoppingCart { ShoppingCartID = shoppingCartId, OrderID = new Guid(), ProductID = new Guid(), Quantity = 3 }
            };
            var mockRepository = new Mock<IShoppingCartRepository>();
            mockRepository.Setup(repo => repo.GetShoppingCartItemsByShoppingCartIdAsync(shoppingCartId)).ReturnsAsync(shoppingCartItems);
            var controller = new ShoppingCartController(mockRepository.Object);

            // Act
            var result = await controller.GetShoppingCartItemsById(shoppingCartId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var shoppingCartList = Assert.IsAssignableFrom<IEnumerable<ShoppingCart>>(okResult.Value as IEnumerable<ShoppingCart>);
            Assert.Collection(shoppingCartList,
                item =>
                {
                    Assert.Equal(new Guid(), item.OrderID);
                    Assert.Equal(new Guid(), item.ProductID);
                    Assert.Equal(2, item.Quantity);
                    Assert.Equal(shoppingCartId, item.ShoppingCartID);
                },
                item =>
                {
                    Assert.Equal(new Guid(), item.OrderID);
                    Assert.Equal(new Guid(), item.ProductID);
                    Assert.Equal(3, item.Quantity);
                    Assert.Equal(shoppingCartId, item.ShoppingCartID);
                }
            );
        }
        [Fact]
        public async Task GetShoppingCartItemsByOrderId_Returns_OkObjectResult_With_ShoppingCartDtoList()
        {
            // Arrange
            var orderId = new Guid();
            var shoppingCartItems = new List<ShoppingCart>
            {
                new ShoppingCart { ShoppingCartID = new Guid(), OrderID = orderId, ProductID = new Guid(), Quantity = 2 },
                new ShoppingCart { ShoppingCartID = new Guid(), OrderID = orderId, ProductID = new Guid(), Quantity = 3 }
            };
            var mockRepository = new Mock<IShoppingCartRepository>();
            mockRepository.Setup(repo => repo.GetShoppingCartItemsByOrderIdAsync(orderId)).ReturnsAsync(shoppingCartItems);
            var controller = new ShoppingCartController(mockRepository.Object);

            // Act
            var result = await controller.GetShoppingCartItemsByOrderId(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var shoppingCartList = Assert.IsAssignableFrom<IEnumerable<ShoppingCart>>(okResult.Value as IEnumerable<ShoppingCart>);
            Assert.Collection(shoppingCartList,
                item =>
                {
                    Assert.Equal(orderId, item.OrderID);
                    Assert.Equal(new Guid(), item.ProductID);
                    Assert.Equal(2, item.Quantity);
                },
                item =>
                {
                    Assert.Equal(orderId, item.OrderID);
                    Assert.Equal(new Guid(), item.ProductID);
                    Assert.Equal(3, item.Quantity);
                }
            );
        }
    }
}
