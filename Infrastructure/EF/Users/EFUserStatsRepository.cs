using Common.Data;
using Common.Entities.Users;
using Infrastructure.Repositories.UserRepositories;

namespace Infrastructure.EF.Users
{
	public class EFUserStatsRepository : IUserStatsRepository
	{
		private readonly DataContext _db;

		public EFUserStatsRepository(DataContext db)
		{
			_db = db;
		}

		public UserStats Create(Guid userReference)
		{
			var newEntry = new UserStats
			{
				UserReference = userReference,
				TradesMade = 0,
				HabitsCompleted = 0,
				PledgesCompleted = 0,
				TradeProfit = 0.0M,
				GiftsGiven = 0
			};

			_db.UserStats.Add(newEntry);
			_db.SaveChanges();

			return newEntry;
		}

		public void EnsureExists(Guid userReference)
		{
			var exists = _db.UserStats.FirstOrDefault(x => x.UserReference == userReference);

			if (exists is null)
				Create(userReference);
		}

		public List<UserStats> GetAll(Guid userReference)
		{
			return _db.UserStats.Where(x => x.UserReference == userReference).ToList();
		}

		public UserStats GetByReference(Guid reference)
		{
			EnsureExists(reference);
			return _db.UserStats.First(x => x.UserReference == reference);
		}

		public UserStats AddTrade(Guid userReference)
		{
			EnsureExists(userReference);

			var entry = GetByReference(userReference);
			entry.TradesMade += 1;
			_db.SaveChanges();

			return entry;
		}

		public UserStats AddHabitComplete(Guid userReference)
		{
			EnsureExists(userReference);

			var entry = GetByReference(userReference);
			entry.HabitsCompleted += 1;
			_db.SaveChanges();

			return entry;
		}

		public UserStats AddPledgeComplete(Guid userReference)
		{
			EnsureExists(userReference);

			var entry = GetByReference(userReference);
			entry.PledgesCompleted += 1;
			_db.SaveChanges();

			return entry;
		}

		public UserStats AddTradeProfit(Guid userReference, decimal profit)
		{
			EnsureExists(userReference);

			var entry = GetByReference(userReference);
			entry.TradeProfit += profit;
			_db.SaveChanges();

			return entry;
		}

		public UserStats AddGiftGiven(Guid userReference)
		{
			EnsureExists(userReference);

			var entry = GetByReference(userReference);
			entry.GiftsGiven += 1;
			_db.SaveChanges();

			return entry;
		}
	}
}
