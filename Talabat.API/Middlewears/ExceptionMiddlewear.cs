using System.Net;
using System.Text.Json;
using Talabat.API.Errors;

namespace Talabat.API.Middlewears
{
	public class ExceptionMiddlewear
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddlewear> _logger;
		private readonly IHostEnvironment _env;

		public ExceptionMiddlewear(RequestDelegate next,ILogger<ExceptionMiddlewear> logger,IHostEnvironment env)
        {
			_next = next;
			_logger = logger;
			_env = env;
		}

		//InvokeAsync 
		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next.Invoke(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

				//if(_env.IsDevelopment())
				//{
				//	var Response = new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString());
				//}
				//else
				//{
				//	var Response = new ApiExceptionResponse(500);
				//}
				var Response = _env.IsDevelopment() ? new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString()) : new ApiExceptionResponse(500);
				var Options = new JsonSerializerOptions()
				{
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase
				};
				var JsonResponse = JsonSerializer.Serialize(Response , Options);
				context.Response.WriteAsync(JsonResponse);
			}
		}
    }
}
