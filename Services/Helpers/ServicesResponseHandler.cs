using Common.Classes;
using Common.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace Services.Helpers
{
	public static class ServicesResponseHandler
	{
		public static ActionResult From(User user, IServicesResponse response, ControllerBase controller)
		{
			if (response.HasError)
			{
				response.Results = "An error occurred";
				return new BadRequestObjectResult(response);
			}

			return new OkObjectResult(response);
		}
	}

	public static class AnonymousServicesResponseHandler
	{
		public static ActionResult From(IServicesResponse response)
		{
			if (response.HasError)
			{
				response.Results = "An error occurred";
				return new BadRequestObjectResult(response);
			}

			return new OkObjectResult(response);
		}
	}
}
