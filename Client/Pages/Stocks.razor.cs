using ApexCharts;
using Client.Components;
using Client.Helpers;
using Client.ServicesBridge;
using Client.States.Achievements;
using Client.States.Toast.Types;
using Common.Classes.Investments;
using Common.DTO.Stocks;
using Common.Entities.Investments;
using Common.Entities.Users;

namespace Client.Pages
{
	public partial class Stocks
	{
		private StocksBridge _stocksBridge;
		private AchievementState _achievementState;

		private List<RawStockResponse> _stocks;
		private List<UserHistoricalStocks> _historical;
		private bool _stocksSelectorModalOpen = false;
		private bool _stocksSellModalOpen = false;
		private ModalType _modalType;
		private RawStockResponse _selectedPurchaseStock;
		private UserInvestments _selectedUserInvestment;
		private List<UserInvestments> _investedCryptos = new();
		private string _userTheme;

		private bool _stocksLoaded = false;
		private bool _investedCryptosLoaded = false;
		private bool _historicalStocksLoaded = false;

		private ApexChartOptions<UserHistoricalStocks> _chartOptions = ChartOptionHelper.GetStocksChartOptions();

		protected override async Task OnInitializedAsync()
		{
			await _localStorage.SetItemAsync("LastPage", "Stocks");

			_stocksBridge = new StocksBridge(_genericHttpClient, _toasterService);
			_achievementState = new AchievementState(_toasterService);

			await _userState.UpdateUser(await LocalStorageHelper.GetAuthToken(_localStorage));

			_userTheme = _userState.GetUserTheme();

			await GetStocks();
		}

		protected async Task GetStocks()
		{
			var jwt = await LocalStorageHelper.GetAuthToken(_localStorage);
			_stocks = await _stocksBridge.GetAllCrypto(jwt);
			_stocks = _stocks.OrderByDescending(x => decimal.Parse(x.askPrice)).ToList();
			_stocksLoaded = true;

			_investedCryptos = await _stocksBridge.GetUserCrypto(jwt);
			_investedCryptosLoaded = true;

			_historical = await _stocksBridge.GetAllHistorical(jwt);
			_historicalStocksLoaded = true;

			await _userState.UpdateUser(jwt);
			StateHasChanged();
		}

		private static string GetMobileValue(decimal initialValue)
		{
			return initialValue.ToString("0,##");
		}

		private async void OnDialogClose(StockPurchaseRequest purchaseRequest)
		{
			_stocksSelectorModalOpen = false;
			_stocksSellModalOpen = false;
			if (purchaseRequest is not null)
			{
				switch (_modalType)
				{
					case ModalType.Assign: await PurchaseStock(purchaseRequest); break;
				}
			}
			StateHasChanged();
		}

		private async void OnDialogClose(StockSellRequest sellRequest)
		{
			_stocksSelectorModalOpen = false;
			_stocksSellModalOpen = false;
			if (sellRequest is not null)
			{
				switch (_modalType)
				{
					case ModalType.Create: await SellStock(sellRequest); break;
				}
			}
			StateHasChanged();
		}

		private void OpenSellDialogModal(UserInvestments stock, ModalType modalType)
		{
			if (stock.Symbol is not null)
			{
				_selectedPurchaseStock = _stocks.First(x => x.symbol == stock.Symbol);
				_modalType = modalType;
				_selectedUserInvestment = stock;
				_stocksSelectorModalOpen = false;
				_stocksSellModalOpen = true;
				StateHasChanged();
			}
		}

		private void OpenDialogModal(RawStockResponse stock, ModalType modalType)
		{
			_modalType = modalType;
			_selectedPurchaseStock = stock;
			_stocksSelectorModalOpen = true;
			_stocksSellModalOpen = false;
			StateHasChanged();
		}

		private async Task PurchaseStock(StockPurchaseRequest request)
		{
			var jwt = await LocalStorageHelper.GetAuthToken(_localStorage);

			if (request.PurchasePrice <= _userState.GetUserBalance())
				await _stocksBridge.PurchaseCrypto(request, jwt);
			else
				_toasterService.AddToast(SimpleToast.NewToast("Purchased Investment", $"Missing balance to invest", MessageColour.Danger, 5));

			await _userState.UpdateUser(jwt);
			await GetStocks();
		}

		private async Task SellStock(StockSellRequest request)
		{
			if (request.Shares <= _selectedUserInvestment.Share)
			{
				var jwt = await LocalStorageHelper.GetAuthToken(_localStorage);
				await _stocksBridge.SellCrypto(request, jwt);
				await _achievementState.TradeProfitUnlock(GetStockProfit(request), jwt);
			}
			else
				_toasterService.AddToast(SimpleToast.NewToast("Sell Investment", $"Missing shares to sell that many", MessageColour.Danger, 5));

			await GetStocks();
		}

		private decimal GetCurrentValue(UserInvestments investment)
		{
			var matchingStock = _stocks.FirstOrDefault(x => x.symbol == investment.Symbol);
			if (matchingStock is not null)
			{
				return decimal.Parse(matchingStock.askPrice) * investment.Share;
			}

			return 0.0M;
		}

		private decimal GetStockProfit(StockSellRequest request)
		{
			var sellTotal = request.SellPrice * request.Shares;
			var matchingStock = _stocks.First(x => x.symbol == request.Symbol);
			var profit = sellTotal - (matchingStock.GetPrice() * request.Shares);

			return profit;
		}
	}
}
