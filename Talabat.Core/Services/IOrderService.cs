using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Order_Aggregation;
using Order = Talabat.Core.Entites.Order_Aggregation.Order;

namespace Talabat.Core.Services
{
	public interface IOrderService
	{
		Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Adress ShippingAdress);

		Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string BuyerEmail);

		Task<Order> GetOrderByIdForSpecificUser(int  OrderId, string BuyerEmail);

		Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethods();
	}
}
