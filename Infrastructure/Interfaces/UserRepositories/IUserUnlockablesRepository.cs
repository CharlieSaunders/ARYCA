using Common.Entities.Users;

namespace Infrastructure.Repositories.UserRepositories
{
	public interface IUserUnlockablesRepository
	{
		public List<UserUnlockables> GetAll(Guid userReference);
		public UserUnlockables Get(Guid userReference, Guid unlockReference);
		public bool Add(Guid userReference, Guid unlockReference);
		public bool DeleteAll(Guid reference);
		public bool DeleteAllUnlocks();
	}
}
