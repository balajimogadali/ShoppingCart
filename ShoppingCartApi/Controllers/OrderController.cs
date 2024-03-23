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
    public class OrderController : ControllerBase
    {
        public readonly IOrderRespository orderRespository;
        public readonly IShoppingCartRepository shoppingCartRepository;
        public OrderController(IOrderRespository orderRespository, IShoppingCartRepository shoppingCartRepository)
        {
            this.orderRespository = orderRespository;
            this.shoppingCartRepository = shoppingCartRepository;
        }

        [HttpPost]
        [Route("CreateOrderByUserID")]
        public async Task<IActionResult> CreateOrderByUserID([FromBody] CreateOrderRequestDto request)
        {
            //Dto to Domain object
            var order = new Order()
            {
                UserID = request.UserID,
                OrderDate = request.OrderDate
            };

            order = await orderRespository.CreateOrderAsync(order);

            var response = new OrderDto()
            {
                OrderID = order.OrderID,
                OrderDate = order.OrderDate,
                UserID = order.UserID

            };

            return Ok(response);

        }

        [HttpGet]
        [Route("{orderId:Guid}")]
        public async Task<IActionResult> GetOrderByOrderID([FromRoute] Guid orderId)
        {
            var order = await orderRespository.GetOrderByOrderIDAsync(orderId);

            if (order is null)
            {
                return NotFound();
            }
            IEnumerable<ShoppingCart> ShoppingCartResponse = await shoppingCartRepository.GetShoppingCartItemsByOrderIdAsync(order.OrderID);
            var response = new OrderDTOResponse()
            {
                OrderID = order.OrderID,
                OrderDate = order.OrderDate,
                UserID = order.UserID,
                TotalAmount = order.TotalAmount,
                Items = ShoppingCartResponse
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("{userId:Guid}")]
        public async Task<IActionResult> GetOrdersByUserID([FromRoute] Guid userId)
        {
            var orders = await orderRespository.GetOrdersByUserIDAsync(userId);
            if (orders is null)
            {
                return NotFound();
            }

            List<Order> ordersList = new List<Order>();
            foreach (var order in orders)
            {
                
                var orderItem = new Order()
                {
                    OrderID = order.OrderID,
                    OrderDate = order.OrderDate,
                    UserID = order.UserID,
                    TotalAmount = order.TotalAmount,
                    Items = (shoppingCartRepository.GetShoppingCartItemsByOrderIdAsync(order.OrderID)==null) ? null: (IEnumerable<ShoppingCart>)shoppingCartRepository.GetShoppingCartItemsByOrderIdAsync(order.OrderID),

                };
                ordersList.Add(orderItem);  
            }

            return Ok(ordersList);
        }



    }
}
