using Common.DTO.Pledges;
using Common.DTO.Stocks;
using Common.DTO.Users;
using Common.Entities.Users;

namespace Infrastructure.Interfaces.UserRepositories
{
	public interface IUserRepositories
	{
		// USERS
		public User GetUserByReference(Guid reference);
		public List<User> GetAllUsers();
		public User GetUserByUsername(string username);
		public User UpdateUser(RepositoryUpdateUserRequest updateUserRequest);
		public void DeleteUser(Guid userReference);
		public User CreateUser(string username, string fname, string sname, string role);
		public void UpdateUserBalance(Guid userReference, decimal value);

		// USER UNLOCKABLES
		public List<UserUnlockables> GetAllUnlockables(Guid userReference);
		public UserUnlockables GetUnlockables(Guid userReference, Guid unlockReference);
		public bool AddUnlockable(Guid userReference, Guid unlockReference);
		public bool DeleteAllUnlockables(Guid reference);
		public bool DeleteAllUnlocks();

		// USER TRANSACTIONS
		public UserTransaction AddTransaction(Guid userReference, string type, decimal value);
		public List<UserTransaction> GetAllTransactionsByReference(Guid userReference);

		// USER STATS
		public void EnsureStatExists(Guid userReference);
		public UserStats GetStatByReference(Guid reference);
		public List<UserStats> GetAllStats(Guid userReference);
		public UserStats AddTradeStat(Guid userReference);
		public UserStats AddHabitCompleteStat(Guid userReference);
		public UserStats AddPledgeCompleteStat(Guid userReference);
		public UserStats AddTradeProfitStat(Guid userReference, decimal profit);
		public UserStats AddGiftGivenStat(Guid userReference);
		public UserStats CreateStat(Guid userReference);

		// USER PLEDGES
		public UserPledges AddPledge(RepositoryAddPledgeRequest addPledgeRequest);
		public List<UserPledges> GetAllPledges(Guid userReference);
		public UserPledges GetPledge(int pledgeId);
		public UserPledges UpdatePledge(UserPledges pledge);
		public void DeletePledge(int pledgeId);

		// USER HABITS 
		public bool AssignHabit(Guid userReference, Guid habitReference);
		public bool UnAssignHabit(Guid userReference, Guid habitReference);
		public bool DeleteHabit(Guid habitReference);
		public bool CompleteHabit(Guid userReference, Guid habitReference);
		public bool HabitExists(Guid userReference, Guid habitReference);
		public UserHabits GetHabit(Guid userReference, Guid habitReference);
		public List<UserHabits> GetHabits(Guid userReference);

		// USER HISTORICAL STOCKS
		public List<UserHistoricalStocks> GetAllHistoricalStocks(Guid userReference);
		public UserHistoricalStocks AddHistoricalStock(RepositoryAddHistoricalStockRequest request);

		// USER ACHIEVEMENTS
		public List<UserAchievements> GetAchievementsForUser(Guid reference);
		public UserAchievements AddAchievement(Guid userReference, int achievementId);
	}
}
