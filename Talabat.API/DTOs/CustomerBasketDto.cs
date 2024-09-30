using Talabat.Core.Entites;

namespace Talabat.API.DTOs
{
	public class CustomerBasketDto
	{
		public string Id { get; set; }
		public List<BaskeItemDto> items { get; set; }
		public string? ClientSecret { get; set; }
		public string? PaymentIntentId { get; set; }
		public int? DeliveryMethodId { get; set; }
	}
}
