using Common.Data;
using Common.DTO.Pledges;
using Common.DTO.Stocks;
using Common.DTO.Users;
using Common.Entities.Users;
using Infrastructure.EF.Users;
using Infrastructure.Interfaces.UserRepositories;
using Infrastructure.Repositories.UserRepositories;

namespace Infrastructure.EF
{
	public class EFUsersRepositories : IUserRepositories
	{
		private readonly IUserHabitRepository _userHabitRepository;
		private readonly IUserHistoricalStocksRepository _userHistoricalStocksRepository;
		private readonly IUserPledgesRepository _userPlegdesRepository;
		private readonly IUserRepository _userRepository;
		private readonly IUserStatsRepository _userStatsRepository;
		private readonly IUserTransactionsRepository _userTransactionRepository;
		private readonly IUserUnlockablesRepository _userUnlockableRepository;
		private readonly IUserAchievementsRepository _userAchievements;

		public EFUsersRepositories(DataContext db)
		{
			_userHabitRepository = new EFUserHabitRepository(db);
			_userHistoricalStocksRepository = new EFUserHistoricalStocksRepository(db);
			_userPlegdesRepository = new EFUserPledgesRepository(db);
			_userRepository = new EFUserRepository(db);
			_userStatsRepository = new EFUserStatsRepository(db);
			_userTransactionRepository = new InMemoryUserTransactionRepository(db);
			_userUnlockableRepository = new EFUserUnlockablesRepository(db);
			_userAchievements = new EFUserAchievementsRepository(db);
		}

		// USERS
		public User GetUserByReference(Guid reference) => _userRepository.GetByReference(reference);
		public List<User> GetAllUsers() => _userRepository.Get();
		public User GetUserByUsername(string username) => _userRepository.GetByUsername(username);
		public User UpdateUser(RepositoryUpdateUserRequest updateUserRequest) => _userRepository.Update(updateUserRequest);
		public void DeleteUser(Guid userReference) => _userRepository.Delete(userReference);
		public User CreateUser(string username, string fname, string sname, string role) => _userRepository.Create(username, fname, sname, role);
		public void UpdateUserBalance(Guid userReference, decimal value) => _userRepository.UpdateBalance(userReference, value);

		// USER UNLOCKABLES
		public List<UserUnlockables> GetAllUnlockables(Guid userReference) => _userUnlockableRepository.GetAll(userReference);
		public UserUnlockables GetUnlockables(Guid userReference, Guid unlockReference) => _userUnlockableRepository.Get(userReference, unlockReference);
		public bool AddUnlockable(Guid userReference, Guid unlockReference) => _userUnlockableRepository.Add(userReference, unlockReference);
		public bool DeleteAllUnlockables(Guid reference) => _userUnlockableRepository.DeleteAll(reference);
		public bool DeleteAllUnlocks() => _userUnlockableRepository.DeleteAllUnlocks();

		// USER TRANSACTIONS
		public UserTransaction AddTransaction(Guid userReference, string type, decimal value) => _userTransactionRepository.Add(userReference, type, value);
		public List<UserTransaction> GetAllTransactionsByReference(Guid userReference) => _userTransactionRepository.GetAllByReference(userReference);

		// USER STATS
		public void EnsureStatExists(Guid userReference) => _userStatsRepository.EnsureExists(userReference);
		public UserStats GetStatByReference(Guid reference) => _userStatsRepository.GetByReference(reference);
		public List<UserStats> GetAllStats(Guid userReference) => _userStatsRepository.GetAll(userReference);
		public UserStats AddTradeStat(Guid userReference) => _userStatsRepository.AddTrade(userReference);
		public UserStats AddHabitCompleteStat(Guid userReference) => _userStatsRepository.AddHabitComplete(userReference);
		public UserStats AddPledgeCompleteStat(Guid userReference) => _userStatsRepository.AddPledgeComplete(userReference);
		public UserStats AddTradeProfitStat(Guid userReference, decimal profit) => _userStatsRepository.AddTradeProfit(userReference, profit);
		public UserStats AddGiftGivenStat(Guid userReference) => _userStatsRepository.AddGiftGiven(userReference);
		public UserStats CreateStat(Guid userReference) => _userStatsRepository.Create(userReference);

		// USER PLEDGES
		public UserPledges AddPledge(RepositoryAddPledgeRequest addPledgeRequest) => _userPlegdesRepository.Add(addPledgeRequest);
		public List<UserPledges> GetAllPledges(Guid userReference) => _userPlegdesRepository.GetAll(userReference);
		public UserPledges GetPledge(int pledgeId) => _userPlegdesRepository.Get(pledgeId);
		public UserPledges UpdatePledge(UserPledges pledge) => _userPlegdesRepository.Update(pledge);
		public void DeletePledge(int pledgeId) => _userPlegdesRepository.Delete(pledgeId);

		// USER HABITS 
		public bool AssignHabit(Guid userReference, Guid habitReference) => _userHabitRepository.Assign(userReference, habitReference);
		public bool UnAssignHabit(Guid userReference, Guid habitReference) => _userHabitRepository.UnAssign(userReference, habitReference);
		public bool DeleteHabit(Guid habitReference) => _userHabitRepository.Delete(habitReference);
		public bool CompleteHabit(Guid userReference, Guid habitReference) => _userHabitRepository.Complete(userReference, habitReference);
		public bool HabitExists(Guid userReference, Guid habitReference) => _userHabitRepository.Exists(userReference, habitReference);
		public UserHabits GetHabit(Guid userReference, Guid habitReference) => _userHabitRepository.GetHabit(userReference, habitReference);
		public List<UserHabits> GetHabits(Guid userReference) => _userHabitRepository.Get(userReference);

		// USER HISTORICAL STOCKS
		public List<UserHistoricalStocks> GetAllHistoricalStocks(Guid userReference) => _userHistoricalStocksRepository.GetAll(userReference);
		public UserHistoricalStocks AddHistoricalStock(RepositoryAddHistoricalStockRequest request) => _userHistoricalStocksRepository.Add(request);

		// USER ACHIEVEMENTS
		public List<UserAchievements> GetAchievementsForUser(Guid reference) => _userAchievements.GetForUser(reference);
		public UserAchievements AddAchievement(Guid userReference, int achievementId) => _userAchievements.Add(userReference, achievementId);

	}
}
