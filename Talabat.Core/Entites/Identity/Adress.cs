using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entites.Identity
{
	public class Adress
	{
		public int Id { get; set; }
		public string Fname { get; set; }
		public string Lname { get; set; }
		public string City { get; set; }
		public string Street { get; set; }
		public string Country { get; set; }
		public AppUser AppUser { get; set; }
		public string AppUserId { get; set; }

	}
}
