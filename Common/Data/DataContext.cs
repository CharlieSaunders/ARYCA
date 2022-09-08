using Common.Entities.Achievements;
using Common.Entities.Configuration;
using Common.Entities.Habits;
using Common.Entities.Investments;
using Common.Entities.Logs;
using Common.Entities.Pledges;
using Common.Entities.Unlockables;
using Common.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Common.Data
{
	public sealed class DataContext : DbContext
	{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public DataContext(DbContextOptions options) : base(options)
		{

		}

		public DbSet<ApplicationConfiguration> AppConfig { get; set; }
		public DbSet<Log> Logs { get; set; }

		public DbSet<User> Users { get; set; }
		public DbSet<Habit> Habits { get; set; }
		public DbSet<Pledge> Pledges { get; set; }
		public DbSet<Unlockable> Unlockables { get; set; }
		public DbSet<Achievement> Achievements { get; set; }

		public DbSet<UserHabits> UserHabits { get; set; }
		public DbSet<UserPledges> UserPledges { get; set; }
		public DbSet<UserUnlockables> UserUnlockables { get; set; }
		public DbSet<UserInvestments> UserInvestments { get; set; }
		public DbSet<UserTransaction> UserTransactions { get; set; }
		public DbSet<UserStats> UserStats { get; set; }
		public DbSet<UserHistoricalStocks> UserHistoricalStocks { get; set; }
		public DbSet<UserAchievements> UserAchievements { get; set; }

#pragma warning restore CS8618
	}
}