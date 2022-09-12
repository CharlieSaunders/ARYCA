using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Common.HttpClients
{
	public static class HttpClientRequestBuilder
	{
		public static HttpContent Request(dynamic request)
		{
			var requestObject = JsonConvert.SerializeObject(request);
			var buffer = Encoding.UTF8.GetBytes(requestObject);
			var byteContent = new ByteArrayContent(buffer);
			byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			return byteContent;
		}

		public static HttpClient AddJwtHeader(HttpClient httpClient, string jwToken)
		{
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwToken);
			return httpClient;
		}
	}
}

