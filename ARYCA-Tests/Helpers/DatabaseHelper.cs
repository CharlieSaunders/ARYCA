using Common.Data;
using Common.Entities.Achievements;
using Common.Entities.Habits;
using Common.Entities.Investments;
using Common.Entities.Logs;
using Common.Entities.Pledges;
using Common.Entities.Unlockables;
using Common.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace ARYCA_Tests.Helpers
{
	public static class DatabaseHelper
	{
		public static DataContext GetDatabase()
		{
			return new DataContext(GetDatabaseOptions());
		}

		public static DataContext SeedUsers(DataContext context, List<User> users)
		{
			foreach (var user in users)
			{
				var matchingUser = context.Users.FirstOrDefault(x => x.UserReference == user.UserReference);
				if (matchingUser is null)
					context.Users.Add(user);
			}

			context.SaveChanges();
			return context;
		}

		public static DataContext SeedUsers(DataContext context, User user)
		{
			var matchingUser = context.Users.FirstOrDefault(x => x.UserReference == user.UserReference);
			if (matchingUser is null)
				context.Users.Add(user);

			context.SaveChanges();
			return context;
		}

		public static DataContext SeedPledges(DataContext context, List<Pledge> pledges)
		{
			foreach (var pledge in pledges)
			{
				var matchingPlegde = context.Pledges.FirstOrDefault(x => x.Name == pledge.Name);
				if (matchingPlegde is null)
					context.Pledges.Add(pledge);
			}

			context.SaveChanges();
			return context;
		}

		public static DataContext SeedAssignedPledges(DataContext context, List<UserPledges> pledges)
		{
			foreach (var pledge in pledges)
			{
				var matchingPlegde = context.UserPledges.FirstOrDefault(x => x.Id == pledge.Id);
				if (matchingPlegde is null)
					context.UserPledges.Add(pledge);
			}

			context.SaveChanges();
			return context;
		}

		public static DataContext SeedHabits(DataContext context, Habit habit)
		{
			var matchingHabit = context.Habits.FirstOrDefault(x => x.Reference == habit.Reference);
			if (matchingHabit is null)
				context.Habits.Add(habit);

			context.SaveChanges();
			return context;
		}

		public static DataContext SeedHabits(DataContext context, List<Habit> habits)
		{
			foreach (var habit in habits)
			{
				var matchingHabit = context.Habits.FirstOrDefault(x => x.Reference == habit.Reference);
				if (matchingHabit is null)
					context.Habits.Add(habit);
			}

			context.SaveChanges();
			return context;
		}

		public static DataContext SeedUserHabits(DataContext context, UserHabits habits)
		{
			var matchingHabit = context.UserHabits.FirstOrDefault(x => x.HabitReference == habits.HabitReference && x.UserReference == habits.UserReference);
			if (matchingHabit is null)
				context.UserHabits.Add(habits);

			context.SaveChanges();
			return context;
		}

		public static DataContext SeedUserHabits(DataContext context, List<UserHabits> habits)
		{
			foreach (var habit in habits)
			{
				var matchingHabit = context.UserHabits.FirstOrDefault(x => x.HabitReference == habit.HabitReference && x.UserReference == habit.UserReference);
				if (matchingHabit is null)
					context.UserHabits.Add(habit);
			}

			context.SaveChanges();
			return context;
		}

		public static DataContext SeedUserInvestments(DataContext context, UserInvestments investment)
		{
			var matchingInvestment = context.UserInvestments.FirstOrDefault(x => x.Id == investment.Id);
			if (matchingInvestment is null)
				context.UserInvestments.Add(investment);


			context.SaveChanges();
			return context;
		}

		public static DataContext SeedUserInvestments(DataContext context, List<UserInvestments> investments)
		{
			foreach (var investment in investments)
			{
				var matchingInvestment = context.UserInvestments.FirstOrDefault(x => x.UserReference == investment.UserReference && x.Symbol == investment.Symbol);
				if (matchingInvestment is null)
					context.UserInvestments.Add(investment);
			}

			context.SaveChanges();
			return context;
		}

		public static DataContext SeedUnlockables(DataContext context, Unlockable unlock)
		{
			var matchingUnlock = context.Unlockables.FirstOrDefault(x => x.Reference == unlock.Reference);
			if (matchingUnlock is null)
				context.Unlockables.Add(unlock);

			context.SaveChanges();
			return context;
		}

		public static DataContext SeedUnlockables(DataContext context, List<Unlockable> unlocks)
		{
			foreach (var unlock in unlocks)
			{
				var matchingUnlock = context.Unlockables.FirstOrDefault(x => x.Reference == unlock.Reference);
				if (matchingUnlock is null)
					context.Unlockables.Add(unlock);
			}

			context.SaveChanges();
			return context;
		}

		public static DataContext SeedLogs(DataContext context, Log log)
		{
			var matchingLog = context.Logs.FirstOrDefault(x => x.Id == log.Id);
			if (matchingLog is null)
				context.Logs.Add(log);

			context.SaveChanges();
			return context;
		}

		public static DataContext SeedLogs(DataContext context, List<Log> logs)
		{
			foreach (var log in logs)
			{
				var matchingLog = context.Logs.FirstOrDefault(x => x.Id == log.Id);
				if (matchingLog is null)
					context.Logs.Add(log);
			}

			context.SaveChanges();
			return context;
		}

		public static DataContext SeedAchievements(DataContext context, Achievement achievement)
		{
			var matchingAchievement = context.Achievements.FirstOrDefault(x => x.Title == achievement.Title);
			if (matchingAchievement is null)
				context.Achievements.Add(achievement);

			context.SaveChanges();
			return context;
		}

		public static DataContext SeedAchievements(DataContext context, List<Achievement> achievements)
		{
			foreach (var achievement in achievements)
			{
				var matchingAchievement = context.Achievements.FirstOrDefault(x => x.Title == achievement.Title);
				if (matchingAchievement is null)
					context.Achievements.Add(achievement);
			}

			context.SaveChanges();
			return context;
		}

		private static DbContextOptions GetDatabaseOptions()
		{
			return new DbContextOptionsBuilder<DataContext>()
			.UseInMemoryDatabase(databaseName: "ARYCA")
			.Options;
		}
	}
}
