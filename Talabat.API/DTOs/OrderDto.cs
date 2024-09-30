namespace Talabat.API.DTOs
{
	public class OrderDto
	{
		public string BasketId {  get; set; }
		public int DeliveryMethodId { get; set; }
		public AdressDto ShippingAddress { get; set; }
	}
}
