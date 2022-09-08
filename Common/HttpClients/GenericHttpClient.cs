using Common.Classes;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Common.HttpClients
{
	public class GenericHttpClient
	{
		private readonly HttpClient _httpClient;
		private readonly string _url;
		public GenericHttpClient(string baseAddress)
		{
			_url = baseAddress;
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri(baseAddress)
			};
		}

		public string GetUrl() => _url;

		public async Task<string> GetAsStringAsync(string extendedUrl, bool configureAwait)
		{
			var apiResponse = await _httpClient.GetAsync(extendedUrl).ConfigureAwait(configureAwait);
			return await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(configureAwait);
		}

		public async Task<bool> DeleteAsyncInUrl(string extendedUrl, bool configureAwait, string jwToken)
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwToken);
			var apiResponse = await _httpClient.DeleteAsync(extendedUrl).ConfigureAwait(configureAwait);
			return apiResponse.IsSuccessStatusCode;
		}

		public async Task<bool> PostAsync(string extendedUrl, dynamic request, bool configureAwait, string jwToken)
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwToken);
			var requestObject = JsonConvert.SerializeObject(request);
			var buffer = Encoding.UTF8.GetBytes(requestObject);
			var byteContent = new ByteArrayContent(buffer);
			byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			var apiResponse = await _httpClient.PostAsync(extendedUrl, byteContent).ConfigureAwait(configureAwait);
			return apiResponse.IsSuccessStatusCode;
		}

		public async Task<string> PostAsyncReturnString(string extendedUrl, dynamic request, bool configureAwait)
		{
			var requestObject = JsonConvert.SerializeObject(request);
			var buffer = Encoding.UTF8.GetBytes(requestObject);
			var byteContent = new ByteArrayContent(buffer);
			byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			var apiResponse = await _httpClient.PostAsync(extendedUrl, byteContent).ConfigureAwait(configureAwait);
			return await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(configureAwait);
		}
		public async Task<IServicesResponse> PostAsyncConvertResult(string extendedUrl, dynamic request, bool configureAwait)
		{
			var response = new IServicesResponse();

			var requestObject = JsonConvert.SerializeObject(request);
			var buffer = Encoding.UTF8.GetBytes(requestObject);
			var byteContent = new ByteArrayContent(buffer);
			byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			var apiResponse = await _httpClient.PostAsync(extendedUrl, byteContent).ConfigureAwait(configureAwait);
			var apiContent = await apiResponse.Content.ReadAsStringAsync();

			var deserialized = JsonConvert.DeserializeObject<IServicesResponse>(apiContent);
			if (deserialized is not null)
				response = deserialized;

			return response;
		}

		public async Task<IServicesResponse> GetAsyncConvertResult(string extendedUrl, bool configureAwait, string jwtToken)
		{
			var response = new IServicesResponse();

			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
			var apiResponse = await _httpClient.GetAsync(extendedUrl).ConfigureAwait(configureAwait);
			var apiContent = await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(configureAwait);

			var deserialized = JsonConvert.DeserializeObject<IServicesResponse>(apiContent);
			if (deserialized is not null)
				response = deserialized;

			return response;
		}

		public async Task<bool> PutAsync(string extendedUrl, dynamic request, bool configureAwait, string jwToken)
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwToken);
			var requestObject = JsonConvert.SerializeObject(request);
			var buffer = Encoding.UTF8.GetBytes(requestObject);
			var byteContent = new ByteArrayContent(buffer);
			byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			var apiResponse = await _httpClient.PutAsync(extendedUrl, byteContent).ConfigureAwait(configureAwait);
			return apiResponse.IsSuccessStatusCode;
		}
	}
}