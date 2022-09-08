using Common.Entities.Achievements;

namespace Common.Classes.Achievements
{
	public static class AchievementHelper
	{
		#region Hardcoded Achievements
		private static readonly List<Achievement> HardcodedAchievements = new()
		{
			new Achievement(1, AchievementType.TradeProfit1, "Complete a trade that has a value of at least 💎1", "TradeProfit1.svg"),
			new Achievement(2, AchievementType.TradeProfit5, "Complete a trade that has a value of at least 💎5", "TradeProfit5.svg"),
			new Achievement(3, AchievementType.TradeProfit10, "Complete a trade that has a value of at least 💎10", "TradeProfit10.svg"),
			new Achievement(4, AchievementType.TradeProfit50, "Complete a trade that has a value of at least 💎50", "TradeProfit50.svg"),
			new Achievement(5, AchievementType.TradeProfit100, "Complete a trade that has a value of at least 💎100.\rYou will have also unlocked the Poly Wolf avatar!", "TradeProfit100.svg"),
			new Achievement(6, AchievementType.TradeProfit1000, "Complete a trade that has a value of at least 💎1000.\rYou will have also unlocked the 💵 🤑 💲 Particles!", "TradeProfit1000.svg"),
		};

		public struct AchievementType
		{
			public const string TradeProfit1 = "Trade Profit 💎1";
			public const string TradeProfit5 = "Trade Profit 💎5";
			public const string TradeProfit10 = "Trade Profit 💎10";
			public const string TradeProfit50 = "Trade Profit 💎50";
			public const string TradeProfit100 = "Trade Profit 💎100";
			public const string TradeProfit1000 = "Trade Profit 💎1000";
		}

		#endregion

		public static List<Achievement> GetAll() => HardcodedAchievements;

		public static Achievement GetAchievement(string type)
		{
			var exists = HardcodedAchievements.FirstOrDefault(x => x.Title == type);
			return exists is not null ? exists : new Achievement();
		}
	}
}


