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

namespace Talabat.Services
{
	public class OrderServices : IOrderService
	{
		private readonly IBasketRepository _basketRepo;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IPaymentService _paymentService;

		public OrderServices(IBasketRepository basketRepo,
			                 IUnitOfWork unitOfWork,
							 IPaymentService paymentService)
        {
			_basketRepo = basketRepo;
			_unitOfWork = unitOfWork;
			_paymentService = paymentService;
		}
        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Adress ShippingAdress)
		{
			//Get Basket Using BasketRepo
			var Basket = await _basketRepo.GetBasketAsync(BasketId);

			//Create List Of Order Items
			var OrderItems = new List<OrderItem>();

			// Add Items To List We Created
			if(Basket.items.Count > 0)
			{
				foreach(var item in Basket.items)
				{
					var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
					var ProductItemOrdered = new ProductItemOrdered(Product.Id, Product.Name , Product.PictureUrl);
					var OrderItem = new OrderItem(ProductItemOrdered, Product.Price, item.Quantity);
					OrderItems.Add(OrderItem);
				}
			}
			//Get Delivery Method From _deliveryRepo (DeliveryMethodId)
			var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);

			var SubTotal = OrderItems.Sum(item => item.Price * item.Quantity);

			var Spec = new OrderWithPaymentIntentSpecifications(Basket.PaymentIntentId);
			var ExOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
			if (ExOrder is not null)
			{
				_unitOfWork.Repository<Order>().Delete(ExOrder);
				await _paymentService.CreateOrUpdatePaymentIntent(BasketId);
			};

			//Create Order
			var Order = new Order(BuyerEmail, ShippingAdress, DeliveryMethod, OrderItems, SubTotal,Basket.PaymentIntentId);

			//Add Order Locally
			await _unitOfWork.Repository<Order>().Add(Order);

			//Add To Databse
			var Result = await _unitOfWork.CompleteAsync();
			if(Result <= 0)
			{
				return null;
			}

			return Order;
		}

		public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethods()
		{
			var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
			return DeliveryMethod;
		}

		public async Task<Order> GetOrderByIdForSpecificUser(int OrderId, string BuyerEmail)
		{
			var Spec = new OrderSepcifications(BuyerEmail, OrderId);
			var Order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
			return Order;
		}

		public async Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string BuyerEmail)
		{
			var Spec = new OrderSepcifications(BuyerEmail);
			var Orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(Spec);
			return Orders;
		}
	}
}
