using Client.States.Toast;
using Client.States.Toast.Types;
using Common.Classes.Investments;
using Common.DTO.Stocks;
using Common.Entities.Investments;
using Common.Entities.Users;
using Common.HttpClients;
using Newtonsoft.Json;

namespace Client.ServicesBridge
{
	public class StocksBridge
	{
		private readonly IGenericHttpClient _genericHttpClient;
		private readonly ToasterService _toasterService;
		private readonly string _url = "Public/Stocks";

		public StocksBridge(IGenericHttpClient httpClient, ToasterService toasterService)
		{
			_toasterService = toasterService;
			_genericHttpClient = httpClient;
		}

		public async Task<List<RawStockResponse>> GetAllCrypto(string jwToken)
		{
			var cryptos = new List<RawStockResponse>();

			try
			{
				var apiResponse = await _genericHttpClient.GetAsyncConvertResult(_url, jwToken);

				if (apiResponse is not null)
					cryptos = JsonConvert.DeserializeAnonymousType<List<RawStockResponse>>(apiResponse.Results.ToString(), cryptos);
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Get Cryptos", $"Unknown error getting all crypto's", MessageColour.Danger, 5));
			}

			return cryptos;
		}

		public async Task<List<UserHistoricalStocks>> GetAllHistorical(string jwToken)
		{
			var historical = new List<UserHistoricalStocks>();

			try
			{
				var apiResponse = await _genericHttpClient.GetAsyncConvertResult($"{_url}/Historical", jwToken);

				if (apiResponse is not null)
					historical = JsonConvert.DeserializeAnonymousType<List<UserHistoricalStocks>>(apiResponse.Results.ToString(), historical);
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Get Historical", $"Unknown error getting all historical crypto's", MessageColour.Danger, 5));
			}

			return historical;
		}

		public async Task PurchaseCrypto(StockPurchaseRequest request, string jwToken)
		{
			var purchase = false;
			try
			{
				purchase = await _genericHttpClient.PutAsync($"{_url}/Purchase", request, jwToken);
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Purchase Crypto", $"Unknown error purchasing crypto", MessageColour.Danger, 5));
			}

			if (purchase)
				_toasterService.AddToast(SimpleToast.NewToast("Investing Purchase", $"Successfully invested in {request.Symbol}", MessageColour.Success, 5));
			else
				_toasterService.AddToast(SimpleToast.NewToast("Investing Purchase", $"Failed to invest in {request.Symbol}", MessageColour.Danger, 5));
		}

		public async Task SellCrypto(StockSellRequest request, string jwToken)
		{
			var sold = false;
			try
			{
				sold = await _genericHttpClient.PutAsync($"{_url}/Sell", request, jwToken);
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Sell Crypto", $"Unknown error selling crypto", MessageColour.Danger, 5));
			}

			if (sold)
				_toasterService.AddToast(SimpleToast.NewToast("Investing Sale", $"Successfully sold {request.Symbol}", MessageColour.Success, 5));
			else
				_toasterService.AddToast(SimpleToast.NewToast("Investing Sale", $"Failed to sell {request.Symbol}", MessageColour.Danger, 5));
		}

		public async Task<List<UserInvestments>> GetUserCrypto(string jwToken)
		{
			var investments = new List<UserInvestments>();
			try
			{
				var apiResponse = await _genericHttpClient.GetAsyncConvertResult($"{_url}/Purchased", jwToken);

				if (apiResponse is not null)
					investments = JsonConvert.DeserializeAnonymousType<List<UserInvestments>>(apiResponse.Results.ToString(), investments);
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Get User Crypto", $"Unknown error getting users crypto", MessageColour.Danger, 5));
			}

			return investments;
		}
	}
}
