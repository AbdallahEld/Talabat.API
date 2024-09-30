using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Order_Aggregation;

namespace Talabat.Core.Services
{
	public interface IPaymentService
	{
		Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId);
		Task<Order> UpdatePaymentIntentIdToSuccedOrFailed(string PaymentIntentId, bool Flag);
	}
}
