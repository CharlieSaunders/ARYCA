using Client.ServicesBridge;
using Common.Classes.Users;
using Common.Entities.Users;
using Common.HttpClients;
using static Common.Entities.Users.User;

namespace Client.States
{
	public class UserState
	{
		private UserResponse AppUser { get; set; }

		private readonly FlatUsersBridge _userBridge;

		public UserState()
		{
			_userBridge = new FlatUsersBridge(new GenericHttpClient("http://api-container:9999"));
			AppUser = new UserResponse();
		}

		public void SetUser(User user) => AppUser = new UserResponse(user);

		public void SetUser(UserResponse user) => AppUser = user;

		public void Logout() => AppUser = new UserResponse();

		public async Task UpdateUser(string jwToken) => SetUser(await _userBridge.GetUserByReference(AppUser.UserReference, jwToken));

		public Guid GetUserReference() => AppUser.UserReference;

		public string GetUsername() => AppUser.Username;

		public string GetFirstName() => AppUser.FirstName;

		public string GetSecondName() => AppUser.SecondName;

		public string GetFullName() => $"{GetFirstName()} {GetSecondName()}";

		public string GetShortName() => $"{AppUser.Title} {GetFirstName()}";

		public string GetUserBalanceString() => AppUser.Balance.ToString("0.00");

		public decimal GetUserBalance() => AppUser.Balance;

		public string GetUserTheme() => AppUser.Theme;

		public bool IsValidUser() => AppUser.IsValidUser();

		public string GetParticleEffect() => AppUser.ParticleEffect;

		public string GetFontFamily() => AppUser.FontFamily;

		public string GetUserRole() => AppUser.Role;

		public bool IsAdmin() => AppUser.Role == UserRole.Admin;

		public string GetAvatarUrl()
		{
			if (!AppUser.IsValidUser())
				return "favicon.png";
			else
				return AppUser.AvatarUrl;
		}
	}
}
