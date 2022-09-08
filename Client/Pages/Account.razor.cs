using Client.Components;
using Client.Helpers;
using Client.ServicesBridge;
using Common.Classes.Users;
using Common.DTO.Achievements;
using Common.DTO.Unlockables;
using Common.DTO.Users;
using Common.Entities.Achievements;
using Common.Entities.Unlockables;
namespace Client.Pages
{
	public partial class Account
	{
		private UsersBridge _userBridge;
		private UnlockablesBridge _unlockablesBridge;
		private AchievementsBridge _achievementsBridge;

		private List<UnlockableResponse> _unlockedThemes;
		private List<UnlockableResponse> _unlockedAvatars;
		private List<UnlockableResponse> _unlockedTitles;
		private List<UnlockableResponse> _unlockedParticles;
		private List<UnlockableResponse> _unlockedFonts;

		private List<UserAchievementResponse> _userAchievements;
		private List<Achievement> _achievements;

		private bool _userRetreived;
		private bool _avatarSelectorModalOpen = false;
		private ModalType _modalType;

		private UserResponse _user = new();
		private string _currentAvatarUrl;


		protected override async Task OnInitializedAsync()
		{
			await _localStorage.SetItemAsync("LastPage", "Account");

			_userBridge = new UsersBridge(_genericHttpClient, _toasterService);
			_unlockablesBridge = new UnlockablesBridge(_genericHttpClient, _toasterService);
			_achievementsBridge = new AchievementsBridge(_genericHttpClient, _toasterService);

			await _userState.UpdateUser(await LocalStorageHelper.GetAuthToken(_localStorage));

			await GetUser();
			await GetUnlocks();
			await GetAchievements();
		}

		private string GetUserTheme(string value)
		{
			if (_unlockedThemes is not null && _unlockedThemes.Count > 0)
			{
				var theme = _unlockedThemes.FirstOrDefault(x => x.Value == value);

				if (theme is null)
					return String.Empty;
				else
					return theme.Name;
			}
			else
				return String.Empty;
		}

		private void OnDialogClose(string avatarUrl)
		{
			_avatarSelectorModalOpen = false;
			if (avatarUrl is not null && avatarUrl != _user.AvatarUrl)
				_user.AvatarUrl = avatarUrl;

			StateHasChanged();
		}

		private void OpenDialogModal(string avatarUrl, ModalType modalType)
		{
			_currentAvatarUrl = avatarUrl;
			_modalType = modalType;
			_avatarSelectorModalOpen = true;
			StateHasChanged();
		}

		protected async Task GetUser()
		{
			_user = await _userBridge.GetUserByReference(_userState.GetUserReference(), await LocalStorageHelper.GetAuthToken(_localStorage));
			_userRetreived = true;
			StateHasChanged();
		}

		protected async Task GetAchievements()
		{
			var authToken = await LocalStorageHelper.GetAuthToken(_localStorage);

			_achievements = await _achievementsBridge.GetAchievements(authToken);

			var userAchievements = await _achievementsBridge.GetUserAchievements(authToken);
			_userAchievements = userAchievements.OrderByDescending(x => x.TimesUnlocked).ToList();
			StateHasChanged();
		}

		protected async Task GetUnlocks()
		{
			var authToken = await LocalStorageHelper.GetAuthToken(_localStorage);
			var allUnlocks = await _unlockablesBridge.GetAllUnlocked(authToken);
			_unlockedAvatars = allUnlocks.Where(x => x.Type == UnlockableTypes.AvatarUrl).OrderBy(x => x.Name).ToList();
			_unlockedThemes = allUnlocks.Where(x => x.Type == UnlockableTypes.Theme).OrderBy(x => x.Name).ToList();
			_unlockedTitles = allUnlocks.Where(x => x.Type == UnlockableTypes.Title).OrderBy(x => x.Name).ToList();
			_unlockedParticles = allUnlocks.Where(x => x.Type == UnlockableTypes.Particle).OrderBy(x => x.Name).ToList();
			_unlockedFonts = allUnlocks.Where(x => x.Type == UnlockableTypes.Font).OrderBy(x => x.Name).ToList();
			StateHasChanged();
		}

		protected async Task Update()
		{
			var updateUserRequest = new UpdateUserRequest
			{
				Username = _user.Username,
				FirstName = _user.FirstName,
				SecondName = _user.SecondName,
				AvatarUrl = _user.AvatarUrl,
				Theme = _user.Theme,
				Title = _user.Title,
				ParticleEffect = _user.ParticleEffect,
				FontFamily = _user.FontFamily
			};

			var jwt = await LocalStorageHelper.GetAuthToken(_localStorage);

			await _localStorage.SetItemAsStringAsync("Username", updateUserRequest.Username);
			await _userBridge.Update(updateUserRequest, jwt);
			await _userState.UpdateUser(jwt);
			await GetUser();
		}
	}
}
