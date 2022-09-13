using Common.Entities.Achievements;

namespace Infrastructure.Interfaces
{
	public interface IAchievementRepository
	{
		public List<Achievement> GetAll();
		public Achievement Add(string title, int identifier, string description, string imageUrl);
		public Achievement GetById(int id);
	}
}
