using Common.Entities.Users;

namespace Common.DTO.Stocks
{
	public class RepositoryAddHistoricalStockRequest
	{
		public Guid UserReference { get; set; }
		public string Symbol { get; set; }
		public decimal Shares { get; set; }
		public decimal Profit { get; set; }

		public RepositoryAddHistoricalStockRequest()
		{

		}

		public RepositoryAddHistoricalStockRequest(User user, StockSellRequest request, decimal profit)
		{
			UserReference = user.UserReference;
			Symbol = request.Symbol;
			Shares = request.Shares;
			Profit = profit;
		}
	}
}
