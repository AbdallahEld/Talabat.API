using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.DTOs;
using Talabat.API.Errors;
using Talabat.Core.Entites;
using Talabat.Core.Repositories;

namespace Talabat.API.Controllers
{
	public class BasketsController : ApiBaseController
	{
		private readonly IBasketRepository _basketRepo;
		private readonly IMapper _mapper;

		public BasketsController(IBasketRepository basketRepo , IMapper mapper)
        {
			_basketRepo = basketRepo;
			_mapper = mapper;
		}
        // Get Or Recreate
        [HttpGet("{BasketId}")] 
		public async Task<ActionResult<CustomerBasket>> GetBasket (string BasketId)
		{
			var Basket = await _basketRepo.GetBasketAsync(BasketId);
			if (Basket is null)
			{
				return Basket = new CustomerBasket(BasketId);
			}
			return Basket;
		}
		// Update Or Create 
		[HttpPost]
		public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto Basket)
		{
			var MappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(Basket);
			var UpdatedOrCreated = await _basketRepo.UpdateBasketAsync(MappedBasket);
			if (UpdatedOrCreated is null)
			{
				return BadRequest(new ApiResponse(400));
			}
			return UpdatedOrCreated;
		}
		// Delete Basket 
		[HttpDelete]
		public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
		{
			return await _basketRepo.DeleteBasketAsync(BasketId);
		}
	}
}
