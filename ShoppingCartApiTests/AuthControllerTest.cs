using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppingCartApi.Controllers;
using ShoppingCartApi.Models.DTO;
using ShoppingCartApi.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartApiTests
{
    public class AuthControllerTest
    {
        [Fact]
        public async Task Login_ValidCredentials_Returns_OkObjectResult_With_LoginResponseDto()
        {
            // Arrange
            var request = new LoginRequestDto { Email = "test@example.com", Password = "password" };
            var identityUser = new IdentityUser { Id = Guid.NewGuid().ToString(), Email = request.Email };
            var roles = new List<string> { "Role1", "Role2" };
            var jwtToken = "sampleJwtToken";

            var mockUserManager = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            mockUserManager.Setup(m => m.FindByEmailAsync(request.Email)).ReturnsAsync(identityUser);
            mockUserManager.Setup(m => m.CheckPasswordAsync(identityUser, request.Password)).ReturnsAsync(true);
            mockUserManager.Setup(m => m.GetRolesAsync(identityUser)).ReturnsAsync(roles);

            var mockTokenRepository = new Mock<ITokenRepository>();
            mockTokenRepository.Setup(repo => repo.CreateJwtToken(identityUser, roles)).Returns(jwtToken);

            var controller = new AuthController(mockUserManager.Object, mockTokenRepository.Object);

            // Act
            var result = await controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<LoginResponseDto>(okResult.Value);
            Assert.Equal(identityUser.Id, response.UserId);
            Assert.Equal(request.Email, response.Email);
            Assert.Equal(roles, response.Roles);
            Assert.Equal(jwtToken, response.Token);
        }

        [Fact]
        public async Task Login_InvalidCredentials_Returns_ValidationProblem()
        {
            // Arrange
            var request = new LoginRequestDto { Email = "test@example.com", Password = "password" };
            var mockUserManager = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            mockUserManager.Setup(m => m.FindByEmailAsync(request.Email)).ReturnsAsync((IdentityUser)null);

            var controller = new AuthController(mockUserManager.Object, null);

            // Act
            var result = await controller.Login(request);
            Microsoft.AspNetCore.Mvc.ObjectResult objectResult = (Microsoft.AspNetCore.Mvc.ObjectResult)result;
            var error= ((Microsoft.AspNetCore.Mvc.ValidationProblemDetails)objectResult.Value).Errors;


            // Assert            
            Assert.Equal(1, error.Count);
 
        }

        [Fact]
        public async Task Register_ValidRequest_Returns_OkResult()
        {
            // Arrange
            var request = new RegisterRequestDto { Email = "test@example.com", Password = "password" };
            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), request.Password)).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<IdentityUser>(), "Reader")).ReturnsAsync(IdentityResult.Success);
            var controller = new AuthController(userManagerMock.Object, null);

            // Act
            var result = await controller.Register(request);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        
    }
}
