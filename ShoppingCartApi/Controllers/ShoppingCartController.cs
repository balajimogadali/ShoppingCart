using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.Models.Domain;
using ShoppingCartApi.Models.DTO;
using ShoppingCartApi.Repositories.Implementation;
using ShoppingCartApi.Repositories.Interface;

namespace ShoppingCartApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        public readonly IShoppingCartRepository shoppingCartRepository;
        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
        }

        [HttpPost]
        [Route("CreateShoppingCart")]
        public async Task<IActionResult> CreateShoppingCart([FromBody] ShoppingCartDto shoppingCartDto)
        {
            //Dto to Domain object
            var shoppingCart = new ShoppingCart()
            {
                OrderID = shoppingCartDto.OrderID,
                ProductID = shoppingCartDto.ProductID,
                Quantity = shoppingCartDto.Quantity,
            };

            var result = await shoppingCartRepository.AddItemToCartAsync(shoppingCart);

            var response = new ShoppingCartDto()
            {
                OrderID = shoppingCartDto.OrderID,
                ProductID = shoppingCartDto.ProductID,
                Quantity = shoppingCartDto.Quantity,
                ShoppingCartID = result.ShoppingCartID
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("UpdateShoppingCart")]
        public async Task<IActionResult> UpdateShoppingCartQuantity([FromBody] ShoppingCartDto shoppingCartDto)
        {
            //Dto to Domain object
            var shoppingCart = new ShoppingCart()
            {
                OrderID = shoppingCartDto.OrderID,
                ProductID = shoppingCartDto.ProductID,
                Quantity = shoppingCartDto.Quantity,
                ShoppingCartID = shoppingCartDto.ShoppingCartID
            };

            var result = await shoppingCartRepository.UpdateCartItemQuantityAsync(shoppingCart);

            var response = new ShoppingCartDto()
            {
                OrderID = shoppingCartDto.OrderID,
                ProductID = shoppingCartDto.ProductID,
                Quantity = shoppingCartDto.Quantity,
                ShoppingCartID = result.ShoppingCartID
            };

            return Ok(response);

        }

        [HttpDelete]
        [Route("{shoppingCartId:Guid}")]
        public async Task<IActionResult> DeleteShoppingCartById([FromRoute] Guid shoppingCartId)
        {
            var deletedProduct = await shoppingCartRepository.RemoveItemFromCartAsync(shoppingCartId);

            if (deletedProduct == null)
            {
                return NotFound();
            }
            var response = new ShoppingCartDto()
            {
                OrderID = deletedProduct.OrderID,
                ProductID = deletedProduct.ProductID,
                Quantity = deletedProduct.Quantity,
                ShoppingCartID = deletedProduct.ShoppingCartID
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("{shoppingCartId:Guid}")]
        public async Task<ActionResult> GetShoppingCartItemsById([FromRoute] Guid shoppingCartId)
        {
            IEnumerable<ShoppingCart> response = await shoppingCartRepository.GetShoppingCartItemsByShoppingCartIdAsync(shoppingCartId);
            var shoppingCart = new List<ShoppingCartDto>();
            foreach (var item in response)
            {
                shoppingCart.Add(new ShoppingCartDto() { OrderID = item.OrderID, ProductID = item.ProductID,Quantity=item.Quantity,ShoppingCartID=item.ShoppingCartID });
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{orderId:Guid}")]
        public async Task<ActionResult> GetShoppingCartItemsByOrderId([FromRoute] Guid orderId)
        {
            IEnumerable<ShoppingCart> response = await shoppingCartRepository.GetShoppingCartItemsByOrderIdAsync(orderId);
            var shoppingCart = new List<ShoppingCartDto>();
            foreach (var item in response)
            {
                shoppingCart.Add(new ShoppingCartDto() { OrderID = item.OrderID, ProductID = item.ProductID, Quantity = item.Quantity, ShoppingCartID = item.ShoppingCartID });
            }

            return Ok(response);
        }
    }
}
