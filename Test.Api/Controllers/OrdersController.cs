using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ecom.Api.Helper;
using Ecom.Core.DTOS.Orders;
using Ecom.Core.Entities.Order;
using Ecom.Core.Services;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDto orderDTO)
        {
            var Email = User.FindFirst(ClaimTypes.Email).Value;
            var result = await _orderService.CreateOrdersAsync(orderDTO,Email);
            if (result.IsError)
            {
                return Problem(
                        statusCode: 404,
                        title: result.FirstError.Code,
                        detail: result.FirstError.Description
                    );
            }
            return Ok(new ResponseApi(200,data:result.Value));
        }
        [HttpGet]
        public async Task<IActionResult> GetOrdersForUser()
        {
            var Email = User.FindFirst(ClaimTypes.Email).Value;
            var orders = await _orderService.GetAllOrdersForUserAsync(Email);
            return Ok(new ResponseApi(200, data: orders));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrdersById(int id)
        {
            var Email = User.FindFirst(ClaimTypes.Email).Value;
            var orders = await _orderService.GetOrderByIdAsync(id, Email);
            return Ok(new ResponseApi(200, data:orders));
        }
        [HttpGet("DeliveryMethods")]
        public async Task<IActionResult> GetDeliveryMethods()
        {
            var DeliveryMethods = await _orderService.GetDeliveryMethodAsync();
            return Ok(DeliveryMethods);
        }
    }
}
