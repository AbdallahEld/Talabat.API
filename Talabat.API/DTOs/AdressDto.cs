using System.ComponentModel.DataAnnotations;

namespace Talabat.API.DTOs
{
	public class AdressDto
	{
		[Required]
		public string Firstname { get; set; }
		[Required]
		public string Lastname { get; set; }
		[Required]
		public string City { get; set; }
		[Required]
		public string Street { get; set; }
		[Required]
		public string Country { get; set; }
	}
}
