using Common.Classes.Investments;
using Newtonsoft.Json;

namespace Common.HttpClients
{
	public class BinanceHttpClient
	{
		private readonly HttpClient _httpClient;

		public BinanceHttpClient()
		{
			_httpClient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Add("Origin", "any");
			_httpClient.DefaultRequestHeaders.Add("X-Requested-With", "any");
		}

		public async Task<List<RawStockResponse>> GetAll()
		{
			var response = new List<RawStockResponse>();

			var apiResponse = await _httpClient.GetAsync($"https://cs-cors-anywhere.herokuapp.com/https://api2.binance.com/api/v3/ticker/24hr?symbols=%5B%22ETHGBP%22,%22BTCGBP%22,%22ADAGBP%22,%22BNBGBP%22,%22MOVRBUSD%22,%22DOGEGBP%22%5D").ConfigureAwait(false);
			var apiContent = await apiResponse.Content.ReadAsStringAsync();

			var deserialized = JsonConvert.DeserializeObject<List<RawStockResponse>>(apiContent);
			if (deserialized is not null)
				response = deserialized;

			return response;
		}

	}
}
