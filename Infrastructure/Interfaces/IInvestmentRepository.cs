using Common.Entities.Investments;

namespace Infrastructure.Interfaces
{
	public interface IInvestmentRepository
	{
		public List<UserInvestments> GetAllForUser(Guid userReference);
		public UserInvestments Add(Guid userReference, string symbol, decimal share, decimal purchasePrice);
		public UserInvestments Sell(Guid userReference, decimal shares, int id, decimal newSellPrice);
	}
}
