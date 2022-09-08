using Common.Classes.Investments;
using Common.DTO.Stocks;
using Common.Entities.Investments;
using Microsoft.AspNetCore.Components;

namespace Client.Components.Stocks
{
	public partial class InvestmentSellDialogModal
	{
		[Parameter]
		public RawStockResponse MatchingStock { get; set; }

		[Parameter]
		public UserInvestments UserInvestment { get; set; }

		[Parameter]
		public EventCallback<StockSellRequest> OnClose { get; set; }

		[Parameter]
		public ModalType DisplayType { get; set; }

		[Parameter]
		public string Theme { get; set; }

		private StockSellRequest _stockSellRequest = new StockSellRequest();

		private Task ModalCancel()
		{
			return OnClose.InvokeAsync(null);
		}

		private Task ModalOk()
		{
			_stockSellRequest.Symbol = MatchingStock.symbol;
			_stockSellRequest.Shares = UserInvestment.Share;
			_stockSellRequest.Id = UserInvestment.Id;
			_stockSellRequest.SellPrice = decimal.Parse(MatchingStock.askPrice);
			return OnClose.InvokeAsync(_stockSellRequest);
		}

		private decimal GetTotalCost()
		{
			var sellPrice = decimal.Parse(MatchingStock.askPrice) * UserInvestment.Share;
			_stockSellRequest.SellPrice = decimal.Parse(MatchingStock.askPrice);
			return sellPrice;
		}
	}
}
