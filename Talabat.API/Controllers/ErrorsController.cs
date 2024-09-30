using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.Errors;

namespace Talabat.API.Controllers
{
	[Route("errors/{Code}")]
	[ApiController]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class ErrorsController : ControllerBase
	{
		public ActionResult Errors(int Code)
		{
			return NotFound(new ApiResponse(Code));
		}
	}
}
