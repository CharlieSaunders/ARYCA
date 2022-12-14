using Common.Entities.Habits;
using Common.Entities.Users;

namespace ARYCA_Tests.Helpers.TypeHelpers
{
	public static class HabitHelper
	{
		public static Habit GetCompletableHabit()
		{
			return new Habit
			{
				Reference = Guid.Parse("cd2271b6-de15-45da-a940-9a40a3e6106d"),
				Name = "Habit",
				Description = "Habit description",
				Value = 1,
				LastCompleted = DateTime.Now.AddDays(-2),
				Completable = true,
			};
		}

		public static List<Habit> GetCompletableHabitList()
		{
			return new List<Habit>()
			{
				GetCompletableHabit()
			};
		}

		public static UserHabits GetCompletableUserHabit(Guid userReference, Guid habitReference)
		{
			return new UserHabits
			{
				UserReference = userReference,
				HabitReference = habitReference,
				LastCompleted = DateTime.Now.AddDays(-2),
			};
		}
	}
}
