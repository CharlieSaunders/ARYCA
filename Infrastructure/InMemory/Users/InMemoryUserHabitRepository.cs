using Common.Data;
using Common.Entities.Users;
using Infrastructure.Repositories.UserRepositories;

namespace Infrastructure.InMemory.Users
{
	public class InMemoryUserHabitRepository : IUserHabitRepository
	{
		private readonly DataContext _db;
		public InMemoryUserHabitRepository(DataContext db)
		{
			_db = db;
		}

		public UserHabits GetHabit(Guid userReference, Guid habitReference)
		{
			try
			{
				var habit = _db.UserHabits.FirstOrDefault(x => x.UserReference == userReference && x.HabitReference == habitReference);
				if (habit is not null)
					return habit;
				else
					return new UserHabits();
			}
			catch
			{
				return new UserHabits();
			}
		}

		public bool Assign(Guid userReference, Guid habitReference)
		{
			try
			{
				_db.UserHabits.Add(new UserHabits
				{
					UserReference = userReference,
					HabitReference = habitReference
				});

				_db.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool Complete(Guid userReference, Guid habitReference)
		{
			try
			{
				var habit = GetHabit(userReference, habitReference);
				if (habit.HabitReference == habitReference && habit.CanBeCompleted())
				{
					habit.LastCompleted = DateTime.Now;
					_db.SaveChanges();
					return true;
				}

				return false;
			}
			catch
			{
				return false;
			}
		}

		public bool UnAssign(Guid userReference, Guid habitReference)
		{
			try
			{
				var habit = _db.UserHabits.Where(x => x.UserReference == userReference && x.HabitReference == habitReference).FirstOrDefault();

				if (habit is null)
					return false;

				_db.UserHabits.Remove(habit);
				_db.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool Delete(Guid habitReference)
		{
			try
			{
				var habits = _db.UserHabits.Where(x => x.HabitReference == habitReference).ToList();

				if (habits is null || habits.Count == 0)
					return false;

				foreach (var habit in habits)
					_db.UserHabits.Remove(habit);

				_db.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public List<UserHabits> Get(Guid userReference)
		{
			try
			{
				return _db.UserHabits.Where(x => x.UserReference == userReference).ToList();
			}
			catch
			{
				return new List<UserHabits>();
			}
		}

		public bool Exists(Guid userReference, Guid habitReference)
		{
			var habit = GetHabit(userReference, habitReference);
			return habit.HabitReference == habitReference && habit.UserReference == userReference;
		}
	}

}
