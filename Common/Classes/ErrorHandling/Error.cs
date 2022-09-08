namespace Common.Classes.ErrorHandling
{
	public class Error
	{
		public Error()
		{

		}

		public Error(string message, string technicalMessage, int statusCode, string details)
		{
			Message = message;
			TechnicalMessage = technicalMessage;
			StatusCode = statusCode;
			Details = details;
		}

		public Error(string message, string technicalMessage, int statusCode)
		{
			Message = message;
			TechnicalMessage = technicalMessage;
			StatusCode = statusCode;
		}

		public int StatusCode { get; set; }
		public string Message { get; set; }
		public string TechnicalMessage { get; set; }
		public string? Details { get; set; }

		public struct Users
		{
			public const string ServicesUnableToCreateSession = "Unable to create a session for the user";
			public const string NoUsersFound = "No users found for details given";
			public const string UsersFoundDoNotMatch = "Users found for details do not match";
			public const string NoActiveSession = "There is no active session for the user given";
			public const string UserBalanceBelowRequirements = "The user does not have the balance to cover the transaction";
		}

		public struct Habits
		{
			public const string UnableToCreateHabit = "Unable to create habit with given information";
			public const string UnableToDeleteHabit = "Unable to delete habit with given information";
			public const string UnableToAssignHabit = "Unable to assign habit to user with given information";
		}

		public struct MiddleWare
		{
			public const string ErrorDuringMiddleware = "Error during application middleware";
		}

		public struct Pledges
		{
			public const string UnableToFindPledge = "Unable to find pledge with given information";
		}

		public struct Unlockables
		{
			public const string UnlockableAlreadyExists = "Unable to create as unlockable already exists";
			public const string UnlockableDoesntExists = "Unable to find unlock with information given";
			public const string UnlockableDidntUpdate = "Unable to update the unlockable";
			public const string DisabledCreationOfAvatars = "Unable to create a new Avatar Unlockable as image upload is disabled currently.";
		}

		public struct ConfigManager
		{
			public const string DisabledViaConfig = "This process is disabled by configuration";
		}

		public struct Investments
		{
			public const string UnableToAddInvestment = "Unable to add the investment to the portfolio";
		}
	}
}