using Common.Entities.Users;

namespace Infrastructure.Repositories.UserRepositories
{
	public interface IUserStatsRepository
	{
		public void EnsureExists(Guid userReference);
		public UserStats GetByReference(Guid reference);
		public List<UserStats> GetAll(Guid userReference);
		public UserStats AddTrade(Guid userReference);
		public UserStats AddHabitComplete(Guid userReference);
		public UserStats AddPledgeComplete(Guid userReference);
		public UserStats AddTradeProfit(Guid userReference, decimal profit);
		public UserStats AddGiftGiven(Guid userReference);
		public UserStats Create(Guid userReference);
	}
}
