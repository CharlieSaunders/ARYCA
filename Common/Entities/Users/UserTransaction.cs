namespace Common.Entities.Users
{
	public class UserTransaction
	{
		public int Id { get; set; }
		public Guid UserReference { get; set; }
		public decimal Value { get; set; }
		public string Type { get; set; }
		public DateTime Added { get; set; }
	}

	public struct UserTransactionType
	{
		public const string CompleteHabit = "Completed Habit";
		public const string PledgeComplete = "Pledge Complete";
		public const string StockPurchase = "Stock Purchase";
		public const string StockSold = "Stock Sold";
		public const string MarketUnlock = "Market Unlock";
		public const string MarketGift = "Market Gift";
	}
}
