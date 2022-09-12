using Common.Entities.Investments;
using Common.Entities.Users;

namespace ARYCA_Tests.Helpers.TypeHelpers
{
	public static class InvestmentHelper
	{
		public static UserInvestments GetGenericInvestment()
		{
			return new UserInvestments
			{
				Id = 1,
				Symbol = "Symbol",
				Price = 10,
				Share = 100
			};
		}

		public static List<UserInvestments> GetGenericInvestmentsList()
		{
			return new List<UserInvestments>()
			{
				GetGenericInvestment()
			};
		}

		public static UserHistoricalStocks GetHistoricalStock(Guid userReference)
		{
			return new UserHistoricalStocks
			{
				Id = 1,
				Time = "Time",
				UserReference = userReference,
				Symbol = "Symbol",
				Shares = 1,
				Profit = 1,
			};
		}

		public static List<UserHistoricalStocks> GetHistoricalStocksList(Guid userReference)
		{
			return new List<UserHistoricalStocks>()
			{
				GetHistoricalStock(userReference)
			};
		}
	}
}
