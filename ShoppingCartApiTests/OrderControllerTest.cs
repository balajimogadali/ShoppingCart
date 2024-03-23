using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppingCartApi.Controllers;
using ShoppingCartApi.Models.Domain;
using ShoppingCartApi.Models.DTO;
using ShoppingCartApi.Repositories.Implementation;
using ShoppingCartApi.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartApiTests
{
    public class OrderControllerTest
    {
        [Fact]
        public async Task CreateOrderByUserID_Returns_OkObjectResult_With_OrderDto()
        {
            // Arrange
            var request = new CreateOrderRequestDto { UserID = Guid.NewGuid(), OrderDate = DateTime.UtcNow };
            var order = new Order { OrderID = Guid.NewGuid(), UserID = request.UserID, OrderDate = request.OrderDate };
            var mockRepositoryOrderRepo = new Mock<IOrderRespository>();
            mockRepositoryOrderRepo.Setup(repo => repo.CreateOrderAsync(It.IsAny<Order>())).ReturnsAsync(order);
            var mockRepositoryShoppingRepo = new Mock<IShoppingCartRepository>();
            var controller = new OrderController(mockRepositoryOrderRepo.Object, mockRepositoryShoppingRepo.Object);

            // Act
            var result = await controller.CreateOrderByUserID(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<OrderDto>(okResult.Value);
            Assert.Equal(order.OrderID, response.OrderID);
            Assert.Equal(order.UserID, response.UserID);
            Assert.Equal(order.OrderDate, response.OrderDate);
        }

        [Fact]
        public async Task GetOrderByOrderID_Returns_OkObjectResult_With_OrderDtoResponse()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                OrderID = orderId,
                UserID = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                TotalAmount = 100 // Assuming some total amount value for testing
            };

            var shoppingCartItems = new List<ShoppingCart>
            {
                new ShoppingCart { ShoppingCartID = Guid.NewGuid(), OrderID = orderId, ProductID = new Guid(), Quantity = 2 },
                new ShoppingCart { ShoppingCartID = Guid.NewGuid(), OrderID = orderId, ProductID = new Guid(), Quantity = 3 }
            };

            var mockOrderRepository = new Mock<IOrderRespository>();
            mockOrderRepository.Setup(repo => repo.GetOrderByOrderIDAsync(orderId)).ReturnsAsync(order);

            var mockShoppingCartRepository = new Mock<IShoppingCartRepository>();
            mockShoppingCartRepository.Setup(repo => repo.GetShoppingCartItemsByOrderIdAsync(orderId)).ReturnsAsync(shoppingCartItems);

            var controller = new OrderController(mockOrderRepository.Object, mockShoppingCartRepository.Object);

            // Act
            var result = await controller.GetOrderByOrderID(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<OrderDTOResponse>(okResult.Value);
            Assert.Equal(order.OrderID, response.OrderID);
            Assert.Equal(order.UserID, response.UserID);
            Assert.Equal(order.OrderDate, response.OrderDate);
            Assert.Equal(order.TotalAmount, response.TotalAmount);

            // Additional assertions for shopping cart items
            Assert.NotNull(response.Items);
            Assert.Equal(shoppingCartItems.Count, response.Items.Count());
        }

        [Fact]
        public async Task GetOrderByOrderID_Returns_NotFoundResult_When_OrderNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var mockOrderRepository = new Mock<IOrderRespository>();
            mockOrderRepository.Setup(repo => repo.GetOrderByOrderIDAsync(orderId)).ReturnsAsync((Order)null);
            var controller = new OrderController(mockOrderRepository.Object, null); // No need to mock shopping cart repository for NotFound case

            // Act
            var result = await controller.GetOrderByOrderID(orderId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetOrdersByUserID_Returns_NotFoundResult_When_OrdersNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockOrderRepository = new Mock<IOrderRespository>();
            mockOrderRepository.Setup(repo => repo.GetOrdersByUserIDAsync(userId)).ReturnsAsync((List<Order>)null);

            var controller = new OrderController(mockOrderRepository.Object, null); // No need to mock shopping cart repository for NotFound case

            // Act
            var result = await controller.GetOrdersByUserID(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
