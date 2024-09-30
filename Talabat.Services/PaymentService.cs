using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Order_Aggregation;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.OrderSpecs;
using ProductEntity = Talabat.Core.Entites.Product;

namespace Talabat.Services
{
	public class PaymentService : IPaymentService
	{
		private readonly IConfiguration _configuration;
		private readonly IBasketRepository _basketRepo;
		private readonly IUnitOfWork _unitOfWork;

		public PaymentService(IConfiguration configuration 
			                 ,IBasketRepository basketRepo
			                 ,IUnitOfWork unitOfWork)
        {
			_configuration = configuration;
			_basketRepo = basketRepo;
			_unitOfWork = unitOfWork;
		}
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
		{
			StripeConfiguration.ApiKey = _configuration["StripeKeys:SecretKey"];
			
			var Basket = await _basketRepo.GetBasketAsync(BasketId);

			if (Basket is null) return null;

			var ShippingPrice = 0M;
			if (Basket.DeliveryMethodId.HasValue)
			{
				var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
				ShippingPrice = DeliveryMethod.Cost;
			}

			if(Basket.items.Count > 0)
			{
				foreach(var item in Basket.items)
				{
					var Product = await _unitOfWork.Repository<ProductEntity>().GetByIdAsync(item.Id);
					if (item.Price != Product.Price)
					{
						item.Price = Product.Price;
					}
				}
			}
			var SubTotal = Basket.items.Sum(item => item.Price * item.Quantity);

			var Service = new PaymentIntentService();
			var PaymentIntent = new PaymentIntent();
			if(string.IsNullOrEmpty(Basket.PaymentIntentId))
			{
				var Options = new PaymentIntentCreateOptions()
				{
					Amount = (long) SubTotal * 100 + (long) ShippingPrice * 100,
					Currency = "usd",
					PaymentMethodTypes = new List<string> {"card"}
				};
				PaymentIntent = await Service.CreateAsync(Options);
				Basket.PaymentIntentId = PaymentIntent.Id;
				Basket.ClientSecret = PaymentIntent.ClientSecret;
			}
			else
			{
				var Options = new PaymentIntentUpdateOptions()
				{
					Amount = (long)SubTotal * 100 + (long)ShippingPrice * 100,
				};
				PaymentIntent = await Service.UpdateAsync(Basket.PaymentIntentId, Options);
				Basket.PaymentIntentId = PaymentIntent.Id;
				Basket.ClientSecret = PaymentIntent.ClientSecret;
			}
			await _basketRepo.UpdateBasketAsync(Basket);
			return Basket;
		}

		public async Task<Order> UpdatePaymentIntentIdToSuccedOrFailed(string PaymentIntentId, bool Flag)
		{
			var Spec = new OrderWithPaymentIntentSpecifications(PaymentIntentId);
			var Order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
			if(Flag)
			{
				Order.Status = OrderStatus.PaymentReceived;
			}
			else
			{
				Order.Status = OrderStatus.PaymentFailed;
			}
			_unitOfWork.Repository<Order>().Update(Order);
			await _unitOfWork.CompleteAsync();
			return Order;
		}
	}
}
