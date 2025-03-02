using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs.Orders;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class OrdersController : APIBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost] 
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDto)
        {
            var address = _mapper.Map<Address>(orderDto.ShippingAddress);
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, address);

            if (order is null)
                return BadRequest(new ApiResponse(400));

            var orderToReurn = _mapper.Map<Order, OrderToReturnDTO>(order);
            return Ok(orderToReurn);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrdersForUserAsync(buyerEmail);

            var OrdersToReurn = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDTO>>(orders);
            return Ok(OrdersToReurn);
        }

        [ProducesResponseType(typeof(OrderToReturnDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderByIdForUser(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdForUserAsync(id, buyerEmail);
            if(order is null)
                return NotFound(new ApiResponse(404));
            var OrderToReturn = _mapper.Map<Order, OrderToReturnDTO>(order);
            return Ok(OrderToReturn);
        }

        [HttpGet("deliveryMethod")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethod = await _orderService.GetDeliveryMethodsAsync();
            return Ok(deliveryMethod);
        }
    }
}
