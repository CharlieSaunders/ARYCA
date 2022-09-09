namespace Client.States.Toast.Types
{
	public record SimpleToast : ToastableObject
	{
		private TimeSpan ElapsedTime => Posted - DateTimeOffset.Now;

		public readonly DateTimeOffset Posted = DateTimeOffset.Now;
		public DateTimeOffset TimeToBurn { get; init; } = DateTimeOffset.Now.AddSeconds(5);

		public string ElapsedTimeText =>
			ElapsedTime.Seconds > 60
			? $"posted {-ElapsedTime.Minutes} mins ago"
			: $"posted {-ElapsedTime.Seconds} secs ago";

		public static SimpleToast NewToast(string title, string message, MessageColour messageColour, int secsToLive)
			=> new()
			{
				Title = title,
				Message = message,
				MessageColour = messageColour,
				TimeToBurn = DateTimeOffset.Now.AddSeconds(secsToLive)
			};
	}
}
