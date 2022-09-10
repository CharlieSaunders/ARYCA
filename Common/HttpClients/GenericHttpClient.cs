using Common.Classes;
using Common.Config;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Common.HttpClients
{
	public class GenericHttpClient
	{
		private readonly HttpClient _httpClient;
		public GenericHttpClient()
		{
			var servicesApiUrl = GlobalConfigFactory.For().GetApiUrl();
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri(servicesApiUrl)
			};
		}

		public async Task<string> GetAsStringAsync(string url)
		{
			return await _httpClient.GetStringAsync(url);
		}

		public async Task<bool> DeleteAsyncInUrl(string url, string jwToken)
		{
			HttpClientRequestBuilder.AddJwtHeader(_httpClient, jwToken);
			var response = await _httpClient.DeleteAsync(url);
			return response.IsSuccessStatusCode;
		}

		public async Task<bool> PostAsync(string url, dynamic request, string jwToken)
		{
			HttpClientRequestBuilder.AddJwtHeader(_httpClient, jwToken);
			var byteContent = HttpClientRequestBuilder.Request(request);
			var apiResponse = await _httpClient.PostAsync(url, byteContent);
			return apiResponse.IsSuccessStatusCode;
		}

		public async Task<IServicesResponse> PostAsyncAnonymous(string url, dynamic request)
		{
			var byteContent = HttpClientRequestBuilder.Request(request);
			var apiResponse = await _httpClient.PostAsync(url, byteContent);
			return await HttpClientResponseHandler.From(apiResponse);
		}

		public async Task<IServicesResponse> GetAsyncConvertResult(string url, string jwToken)
		{
			HttpClientRequestBuilder.AddJwtHeader(_httpClient, jwToken);
			var apiResponse = await _httpClient.GetAsync(url);
			return await HttpClientResponseHandler.From(apiResponse);
		}

		public async Task<bool> PutAsync(string url, dynamic request, string jwToken)
		{
			HttpClientRequestBuilder.AddJwtHeader(_httpClient, jwToken);
			var byteContent = HttpClientRequestBuilder.Request(request);
			var apiResponse = await _httpClient.PutAsync(url, byteContent);
			return apiResponse.IsSuccessStatusCode;
		}
	}
}