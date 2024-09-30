using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.API.DTOs;
using Talabat.API.Errors;
using Talabat.Core.Entites.Order_Aggregation;
using Talabat.Core.Services;
using OrderAdress = Talabat.Core.Entites.Order_Aggregation.Adress;

namespace Talabat.API.Controllers
{
	public class OrdersController : ApiBaseController
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;

		public OrdersController(IOrderService orderService , IMapper mapper)
        {
			_orderService = orderService;
			_mapper = mapper;
		}
		[ProducesResponseType(typeof(Order) , StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status400BadRequest)]
		[HttpPost]
		[Authorize]
		public async Task<ActionResult<Order>> CreateOrder(OrderDto OrderDto)
		{
			var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
			var MappedAdress = _mapper.Map<AdressDto, OrderAdress>(OrderDto.ShippingAddress);
			var Order = await _orderService.CreateOrderAsync(BuyerEmail, OrderDto.BasketId, OrderDto.DeliveryMethodId, MappedAdress);
			if(Order is null)
			{
				return BadRequest(new ApiResponse(400, "Theres Problem In Order"));
			}
			return Ok(Order);
		}
		[ProducesResponseType(typeof(IReadOnlyList<Order>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet]
		[Authorize]
		public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
		{
			var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
			var Orders = await _orderService.GetOrdersForSpecificUserAsync(BuyerEmail);
			if(Orders is null)
			{
				return NotFound(new ApiResponse(404,"User has no orders"));
			}
			var MappedOrder = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(Orders);
			return Ok(MappedOrder);
		}
		[ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[Authorize]
		[HttpGet("{id}")]
		public async Task<ActionResult<OrderToReturnDto>> GetOrderById(int id)
		{
			var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
			var Order = await _orderService.GetOrderByIdForSpecificUser(id, BuyerEmail);
			if (Order is null)
			{
				return NotFound(new ApiResponse(404, $"no order with this id {id}"));
			}
			var MappedOrder = _mapper.Map<Order, OrderToReturnDto>(Order);
			return Ok(MappedOrder);
		}
		[HttpGet("DeliveryMethods")]
		public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetAllMethods()
		{
			var DeliveryMethods = await _orderService.GetDeliveryMethods();
			return Ok(DeliveryMethods);
		}
	}
}
