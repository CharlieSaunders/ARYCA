using Common.Classes.ErrorHandling;
using System.Net;
using System.Text.Json;

namespace Services.Middleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		public ExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				await context.Response.WriteAsync(CreateErrorResponse(context, ex));
			}
		}

		private static string CreateErrorResponse(HttpContext context, Exception ex)
		{

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

			var stackTrace = ex.StackTrace is null ? "empty stack trace" : ex.StackTrace.ToString();

			var error = new Error(Error.MiddleWare.ErrorDuringMiddleware, ex.Message, context.Response.StatusCode, stackTrace);

			var options = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			return JsonSerializer.Serialize(error, options);
		}
	}
}
