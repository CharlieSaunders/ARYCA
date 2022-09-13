using Common.Entities.Habits;

namespace Infrastructure.Interfaces
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
}
