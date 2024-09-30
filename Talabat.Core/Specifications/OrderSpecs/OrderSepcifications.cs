using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Order_Aggregation;

namespace Talabat.Core.Specifications.OrderSpecs
{
	public class OrderSepcifications : BaseSpecifications<Order>
	{
        public OrderSepcifications(string email):base(O => O.BuyerEmail == email)
        {
            Includes.Add(O =>O.DeliveryMethod);
            Includes.Add(O => O.Items);
            AddOrderByDescending(O => O.OrderDate);
        }
        public OrderSepcifications(string email , int OrderId):base(O => O.BuyerEmail == email && O.Id == OrderId)
        {
			Includes.Add(O => O.DeliveryMethod);
			Includes.Add(O => O.Items);
		}
    }
}
