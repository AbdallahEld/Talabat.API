using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Order_Aggregation;

namespace Talabat.Core.Specifications.OrderSpecs
{
	public class OrderWithPaymentIntentSpecifications : BaseSpecifications<Order>
	{
        public OrderWithPaymentIntentSpecifications(string PaymentIntentId):base(O => O.PaymentIntentId == PaymentIntentId)
        {
            
        }
    }
}
