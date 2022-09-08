namespace Common.DTO.Unlockables
{
	public class UnlockableResponse
	{
		public Guid UnlockableReference { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public string Value { get; set; }
		public bool Unlocked { get; set; }
		public decimal Price { get; set; }
		public bool Display { get; set; }
	}
}
