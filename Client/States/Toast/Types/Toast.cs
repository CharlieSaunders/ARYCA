namespace Client.States.Toast.Types
{
	public record SimpleToast : ToastableObject
	{
		private Guid Id = Guid.NewGuid();
		private TimeSpan elapsedTime => Posted - DateTimeOffset.Now;

		public readonly DateTimeOffset Posted = DateTimeOffset.Now;
		public DateTimeOffset TimeToBurn { get; init; } = DateTimeOffset.Now.AddSeconds(30);

		public string ElapsedTimeText =>
			elapsedTime.Seconds > 60
			? $"posted {-elapsedTime.Minutes} mins ago"
			: $"posted {-elapsedTime.Seconds} secs ago";

		public static SimpleToast NewToast(string title, string message, MessageColour messageColour, int secsToLive)
			=> new SimpleToast
			{
				Title = title,
				Message = message,
				MessageColour = messageColour,
				TimeToBurn = DateTimeOffset.Now.AddSeconds(secsToLive)
			};
	}
}
