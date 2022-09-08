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
	}
}
