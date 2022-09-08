using Common.Data;
using Common.Entities.Achievements;

namespace Infrastructure.Interfaces
{
	public interface IAchievementRepository
	{
		public List<Achievement> GetAll();
		public Achievement Add(string title, int identifier, string description, string imageUrl);
		public Achievement GetById(int id);
	}

	public class AchievementRepository : IAchievementRepository
	{
		private readonly DataContext _db;

		public AchievementRepository(DataContext db)
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
