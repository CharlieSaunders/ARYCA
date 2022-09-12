using Common.DTO.Achievements;

namespace ARYCA_Tests.Helpers.TypeHelpers
{
	public static class AchievementTestHelper
	{

		public static List<UserAchievementResponse> GetUserAchievements()
		{
			return new List<UserAchievementResponse>()
			{
				new UserAchievementResponse
				{
					Title = "Test1",
					Description = "Description1",
					ImageUrl = "ImageUrl1",
					TimesUnlocked = 1
				}
			};
		}
	}
}

