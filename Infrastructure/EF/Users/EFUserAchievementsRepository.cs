using Common.Data;
using Common.Entities.Users;
using Infrastructure.Interfaces.UserRepositories;

namespace Infrastructure.EF.Users
{
	public class EFUserAchievementsRepository : IUserAchievementsRepository
	{
		private readonly DataContext _db;

		public EFUserAchievementsRepository(DataContext db)
		{
			_db = db;
		}

		public UserAchievements Add(Guid userReference, int achievementId)
		{
			var matching = _db.UserAchievements.FirstOrDefault(x => x.UserReference == userReference && x.AchievementReference == achievementId);
			if (matching is null)
			{
				var newUserAchievement = new UserAchievements
				{
					UserReference = userReference,
					AchievementReference = achievementId,
					TimesUnlocked = 1
				};

				_db.UserAchievements.Add(newUserAchievement);
				_db.SaveChanges();
				return newUserAchievement;
			}

			matching.TimesUnlocked += 1;
			_db.SaveChanges();
			return matching;
		}

		public List<UserAchievements> GetForUser(Guid userReference)
		{
			return _db.UserAchievements.Where(x => x.UserReference == userReference).ToList();
		}
	}
}
