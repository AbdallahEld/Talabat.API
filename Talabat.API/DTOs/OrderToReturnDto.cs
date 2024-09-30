using Talabat.Core.Entites.Order_Aggregation;

namespace Talabat.API.DTOs
{
	public class OrderToReturnDto
	{
		public string Id { get; set; }
		public string BuyerEmail { get; set; }
		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
		public string Status { get; set; } 
		public Adress ShipingAdress { get; set; }
		public string DeliveryMethod { get; set; }
		public decimal DeliveryMethodCost { get; set; }
		public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();
		public decimal Subtotal { get; set; }
		public decimal Total { get; set; }

		public string PaymentIntentId {  get; set; }
	}

}
