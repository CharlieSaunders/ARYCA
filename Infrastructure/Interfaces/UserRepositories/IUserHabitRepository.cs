using Common.Entities.Users;

namespace Infrastructure.Repositories.UserRepositories
{
	public interface IUserHabitRepository
	{
		bool Assign(Guid userReference, Guid habitReference);
		bool UnAssign(Guid userReference, Guid habitReference);
		bool Delete(Guid habitReference);
		bool Complete(Guid userReference, Guid habitReference);
		bool Exists(Guid userReference, Guid habitReference);
		UserHabits GetHabit(Guid userReference, Guid habitReference);
		List<UserHabits> Get(Guid userReference);
	}
}
