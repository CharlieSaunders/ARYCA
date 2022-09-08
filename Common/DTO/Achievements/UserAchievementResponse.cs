using Common.Entities.Achievements;
using Common.Entities.Users;
using System.Runtime.Serialization;

namespace Common.DTO.Achievements
{
	[DataContract]
	public class UserAchievementResponse
	{
		[DataMember(EmitDefaultValue = false)]
		public string Title { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string ImageUrl { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public int TimesUnlocked { get; set; }

		public UserAchievementResponse()
		{

		}

		public UserAchievementResponse(UserAchievements userAchievement, Achievement achievement)
		{
			Title = achievement.Title;
			Description = achievement.Description;
			ImageUrl = achievement.ImageUrl;
			TimesUnlocked = userAchievement.TimesUnlocked;
		}
	}
}
