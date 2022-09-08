using Common.Entities.Users;

namespace Infrastructure.Interfaces.UserRepositories
{
	public interface IUserAchievementsRepository
	{
		public List<UserAchievements> GetForUser(Guid userReference);
		public UserAchievements Add(Guid userReference, int achievementId);
	}
}
