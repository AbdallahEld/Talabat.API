using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entites.Order_Aggregation
{
	public class Order : BaseEntity
	{
        public Order()
        {
            
        }
        public Order(string buyerEmail, Adress shipingAdress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subtotal, string paymentIntentId)
		{
			BuyerEmail = buyerEmail;
			ShipingAdress = shipingAdress;
			DeliveryMethod = deliveryMethod;
			Items = items;
			Subtotal = subtotal;
			PaymentIntentId = paymentIntentId;
		}
		public string BuyerEmail { get; set; }
		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
		public OrderStatus Status { get; set; } = OrderStatus.Pending;
		public Adress ShipingAdress { get; set; }
		public DeliveryMethod DeliveryMethod { get; set; }
		public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
		public decimal Subtotal { get; set; }
		public decimal GetTotal()
		{
			return Subtotal + DeliveryMethod.Cost;
		}
		public string PaymentIntentId { get; set; } 
	}
}
