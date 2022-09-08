using Common.Classes.Investments;
using Common.DTO.Stocks;
using Microsoft.AspNetCore.Components;

namespace Client.Components.Stocks
{
	public partial class InvestmentDialogModal
	{
		[Parameter]
		public RawStockResponse SelectedStock { get; set; }

		[Parameter]
		public EventCallback<StockPurchaseRequest> OnClose { get; set; }

		[Parameter]
		public ModalType DisplayType { get; set; }

		[Parameter]
		public string Theme { get; set; }

		private StockPurchaseRequest _stockPurchaseRequest = new StockPurchaseRequest();

		private Task ModalCancel()
		{
			return OnClose.InvokeAsync(null);
		}

		private Task ModalOk()
		{
			_stockPurchaseRequest.Symbol = SelectedStock.symbol;
			return OnClose.InvokeAsync(_stockPurchaseRequest);
		}

		private decimal GetTotalCost()
		{
			var purchasePrice = _stockPurchaseRequest.Share * decimal.Parse(SelectedStock.askPrice);
			_stockPurchaseRequest.PurchasePrice = decimal.Parse(SelectedStock.askPrice);
			return purchasePrice;
		}
	}
}
