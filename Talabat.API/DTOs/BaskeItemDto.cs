using System.ComponentModel.DataAnnotations;

namespace Talabat.API.DTOs
{
	public class BaskeItemDto
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public string productName { get; set; }
		[Required]
		public string PictureUrl { get; set; }
		[Required]
		public string Brand { get; set; }
		[Required]
		public string Type { get; set; }
		[Required]
		[Range(0.1 , double.MaxValue , ErrorMessage = "Price Cannot Be Zero")]
		public decimal Price { get; set; }
		[Required]
		[Range(1 , int.MaxValue ,ErrorMessage = "Quantity Must Be One At Least")]
		public int Quantity { get; set; }
	}
}