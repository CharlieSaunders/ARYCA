using Common.Entities.Achievements;

namespace Client.States.Toast.Types
{
	public record AchievementToast : ToastableObject
	{
		public readonly DateTimeOffset Posted = DateTimeOffset.Now;
		public DateTimeOffset TimeToBurn { get; init; } = DateTimeOffset.Now.AddSeconds(5);

		public static AchievementToast NewToast(Achievement type, int secsToLive)
		{
			var newAchievement = new AchievementToast
			{
				Title = type.Title,
				Message = type.Description,
				ImageUrl = type.ImageUrl,
				MessageColour = MessageColour.Dark,
				TimeToBurn = DateTimeOffset.Now.AddSeconds(secsToLive),
				IsAchievement = true
			};

			return newAchievement;
		}
	}
}
