using Common.Entities.Investments;

namespace ARYCA_Tests.Helpers
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
	}
}
