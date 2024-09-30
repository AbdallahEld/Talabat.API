using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.API.DTOs;
using Talabat.API.Errors;
using Talabat.Core.Entites;
using Talabat.Core.Services;

namespace Talabat.API.Controllers
{
	[Authorize]
	public class PaymentController : ApiBaseController
	{
		private readonly IPaymentService _payment;
		private readonly IMapper _mapper;
		const string endpointSecret = "whsec_853e11944183c5a55142646e921b490cba2974ab269c218fc6d426adb45dfb86";

		public PaymentController(IPaymentService payment , IMapper mapper)
        {
			_payment = payment;
			_mapper = mapper;
		}
		[ProducesResponseType(typeof(CustomerBasketDto) , StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
		[HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
		{
			var CustomerBasket = await _payment.CreateOrUpdatePaymentIntent(basketId);
			if(CustomerBasket == null)
			{
				return BadRequest(new ApiResponse(400, "Theres problem with your cart"));
			}
			var MappedBasket = _mapper.Map<CustomerBasket, CustomerBasketDto>(CustomerBasket);
			return Ok(MappedBasket);
		}
		//use stripe to add endpoint with 2 events if payment intent failed or succedded
		[HttpPost("webhook")]
		public async Task<IActionResult> stripeWebHook()
		{
			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
			try
			{
				var stripeEvent = EventUtility.ConstructEvent(json,
					Request.Headers["Stripe-Signature"], endpointSecret);

				var PaymentIntent = stripeEvent.Data.Object as PaymentIntent;
				// Handle the event
				if(stripeEvent.Type == Events.PaymentIntentPaymentFailed)
				{
					await _payment.UpdatePaymentIntentIdToSuccedOrFailed(PaymentIntent.Id, false);
				}
				else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
				{
					await _payment.UpdatePaymentIntentIdToSuccedOrFailed(PaymentIntent.Id, true);
				} 
				return Ok();
			}
			catch(StripeException e)
			{
				return BadRequest();
			}
		}
		//[HttpPost("webhook")]
	}
}
