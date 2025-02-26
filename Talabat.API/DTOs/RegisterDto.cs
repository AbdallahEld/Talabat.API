﻿using System.ComponentModel.DataAnnotations;

namespace Talabat.API.DTOs
{
	public class RegisterDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string DisplayName { get; set; }
		[Required]
		[Phone]
		public string PhoneNumber { get; set; }
		[Required]
		[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&#^])[A-Za-z\\d@$!%*?&#^]{6,10}$" ,ErrorMessage ="Password Must Contain 1 Uppercase , 1 Lowercase , 1 Digit , 1 Special Character")]
		public string Password { get; set; }
	}
}
