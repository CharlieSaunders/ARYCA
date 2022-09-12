using Common.Classes;
using Common.Config;

namespace Common.HttpClients
{
	public interface IGenericHttpClient
	{
		public Task<bool> DeleteAsyncInUrl(string url, string jwToken);
		public Task<bool> PostAsync(string url, dynamic request, string jwToken);
		public Task<bool> PutAsync(string url, dynamic request, string jwToken);
		public Task<IServicesResponse> PostAsyncAnonymous(string url, dynamic request);
		public Task<IServicesResponse> GetAsyncConvertResult(string url, string jwToken);
	}

	public class GenericHttpClient : IGenericHttpClient
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

		public async Task<bool> DeleteAsyncInUrl(string url, string jwToken)
		{
			HttpClientRequestBuilder.AddJwtHeader(_httpClient, jwToken);
			var response = await HttpClientResponseHandler.From( await _httpClient.DeleteAsync(url));
			return response.Results;
		}

		public async Task<bool> PostAsync(string url, dynamic request, string jwToken)
		{
			HttpClientRequestBuilder.AddJwtHeader(_httpClient, jwToken);
			var byteContent = HttpClientRequestBuilder.Request(request);
			var response = await HttpClientResponseHandler.From(await _httpClient.PostAsync(url, byteContent));
			return response.Results;
		}

		public async Task<bool> PutAsync(string url, dynamic request, string jwToken)
		{
			HttpClientRequestBuilder.AddJwtHeader(_httpClient, jwToken);
			var byteContent = HttpClientRequestBuilder.Request(request);
			var response = await HttpClientResponseHandler.From(await _httpClient.PutAsync(url, byteContent));
			return response.Results;
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
	}
}