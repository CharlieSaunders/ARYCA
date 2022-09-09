namespace Client.States.Toast.Types
{
	public record ToastableObject
	{
		public bool IsBurnt { get; set; }
		public bool IsAchievement { get; set; }
		public string Title { get; set; }
		public string Message { get; set; }
		public string ImageUrl { get; set; }
		public MessageColour MessageColour { get; set; }
	}

	public enum MessageColour
	{
		Primary, Secondary, Dark, Light, Success, Danger, Warning, Info
	}
}
