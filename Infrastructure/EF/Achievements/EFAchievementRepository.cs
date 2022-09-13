using Common.Data;
using Common.Entities.Achievements;
using Infrastructure.Interfaces;

namespace Infrastructure.EF.Achievements
{
	public class EFAchievementRepository : IAchievementRepository
	{
		private readonly DataContext _db;

		public EFAchievementRepository(DataContext db)
		{
			_db = db;
		}

		public Achievement Add(string title, int identifier, string description, string imageUrl)
		{
			var newAchievement = new Achievement(identifier, title, description, imageUrl);
			_db.Achievements.Add(newAchievement);
			_db.SaveChanges();
			return newAchievement;
		}

		public Achievement GetById(int id)
		{
			var achievement = _db.Achievements.FirstOrDefault(x => x.Identifier == id);
			return achievement is not null ? achievement : new Achievement();
		}

		public List<Achievement> GetAll()
		{
			return _db.Achievements.ToList();
		}
	}
}
