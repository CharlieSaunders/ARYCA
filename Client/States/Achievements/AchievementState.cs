using Client.ServicesBridge;
using Client.States.Toast;
using Client.States.Toast.Types;
using Common.Entities.Achievements;
using Common.HttpClients;
using static Common.Classes.Achievements.AchievementHelper;

namespace Client.States.Achievements
{
	public class AchievementState
	{
		private readonly AchievementsBridge _achievementBridge;
		private readonly ToasterService _toasterService;

		public AchievementState(ToasterService toasterService)
		{
			_toasterService = toasterService;
			_achievementBridge = new AchievementsBridge(new GenericHttpClient(), toasterService);
		}

		public async Task Unlock(string type, string jwt)
		{
			var matchingAchievement = GetAchievement(type);
			_toasterService.AddToast(AchievementToast.NewToast(matchingAchievement, 5));

			await _achievementBridge.UnlockAchievement(jwt, matchingAchievement.Identifier);
		}

		public async Task TradeProfitUnlock(decimal profit, string jwt)
		{
			var achievement = new Achievement();

			if (profit >= 1.0M && profit < 5.0M)
				achievement = GetAchievement(AchievementType.TradeProfit1);

			if (profit >= 5.0M && profit < 10.0M)
				achievement = GetAchievement(AchievementType.TradeProfit5);

			if (profit >= 10.0M && profit < 50.0M)
				achievement = GetAchievement(AchievementType.TradeProfit10);

			if (profit >= 50.0M && profit < 100.0M)
				achievement = GetAchievement(AchievementType.TradeProfit50);

			if (profit >= 100.0M && profit < 1000.0M)
				achievement = GetAchievement(AchievementType.TradeProfit100);

			if (profit >= 1000.0M)
				achievement = GetAchievement(AchievementType.TradeProfit1000);

			if (achievement.IsValid())
			{
				await _achievementBridge.UnlockAchievement(jwt, achievement.Identifier);
				_toasterService.AddToast(AchievementToast.NewToast(achievement, 5));
			}
		}
	}
}