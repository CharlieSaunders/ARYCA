using Common.Data;
using Common.Entities.Habits;

namespace Infrastructure.Repositories
{
	public interface IHabitsRepository
	{
		List<Habit> GetAll();
		Habit GetByReference(Guid reference);
		Habit Create(string name, string description, decimal value);
		Habit Update(Guid reference, string name, string description, decimal value);
		bool Delete(Guid reference);
		bool Exists(string name);
		bool Exists(Guid reference);

	}

	public class HabitsRepository : IHabitsRepository
	{
		private readonly DataContext _db;

		public HabitsRepository(DataContext db)
		{
			_db = db;
		}

		public List<Habit> GetAll()
		{
			return _db.Habits.ToList();
		}

		public Habit GetByReference(Guid reference)
		{
			return _db.Habits.First(x => x.Reference == reference);
		}

		public bool Exists(string name)
		{
			var habit = _db.Habits.FirstOrDefault(x => x.Name == name);
			return habit is not null && habit.Reference != Guid.Empty;
		}

		public bool Exists(Guid reference)
		{
			var habit = _db.Habits.FirstOrDefault(x => x.Reference == reference);
			return habit is not null && habit.Name != string.Empty;
		}

		public Habit Create(string name, string description, decimal value)
		{
			var newHabit = new Habit
			{
				Name = name,
				Description = description,
				Value = value,
				Reference = Guid.NewGuid()
			};

			try
			{
				_db.Habits.Add(newHabit);
				_db.SaveChanges();
				return newHabit;
			}
			catch
			{
				return new Habit();
			}
		}

		public bool Delete(Guid reference)
		{
			try
			{
				var habit = GetByReference(reference);
				_db.Habits.Remove(habit);
				_db.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public Habit Update(Guid reference, string name, string description, decimal value)
		{
			try
			{
				var habit = GetByReference(reference);
				habit.Name = name;
				habit.Description = description;
				habit.Value = value;

				_db.SaveChanges();

				return habit;
			}
			catch
			{
				return new Habit();
			}
		}
	}

}
