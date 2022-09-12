using Common.Entities.Users;
using Microsoft.AspNetCore.Http;

namespace ARYCA_Tests.Helpers
{
	public static class HttpContextHelper
	{
		public static HttpContext GetWithArycaUserReference(string userReference)
		{
			var context = new DefaultHttpContext();
			context.Request.Headers["X-ARYCA-UserReference"] = userReference;
			return context;
		}

		public static HttpContext GetWithArycaUserReferenceAndJwt(string userReference, string jwt)
		{
			var context = new DefaultHttpContext();
			context.Request.Headers["X-ARYCA-UserReference"] = userReference;
			context.Request.Headers["Authorization"] = $"Bearer {jwt}";
			return context;
		}

		public static HttpContext GetForUser(User user)
		{
			var context = new DefaultHttpContext();
			if (user.IsValidReference())
				context.Request.Headers["X-ARYCA-UserReference"] = user.UserReference.ToString();

			if (user.AuthenticationToken != String.Empty)
				context.Request.Headers["Authorization"] = $"Bearer {user.AuthenticationToken}";

			return context;
		}
	}
}
