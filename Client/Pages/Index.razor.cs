using Client.Helpers;
using Client.ServicesBridge;
using Common.Classes.Users;
using Common.Entities.Configuration;

namespace Client.Pages
{
	public partial class Index
	{
		private UsersBridge _usersBridge;
		private List<SlimUser> _users;
		private string _userTheme;
		private string[] _relationshipTime;

		private bool _usersLoaded = false;
		private ApplicationConfiguration _appConfig;

		protected override async Task OnInitializedAsync()
		{
			await _localStorage.SetItemAsStringAsync("LastPage", "/");
			_userTheme = _userState.GetUserTheme();

			_usersBridge = new UsersBridge(_genericHttpClient, _toasterService);

			_appConfig = await _localStorage.ContainKeyAsync("ApplicationConfig")
						 ? await _localStorage.GetItemAsync<ApplicationConfiguration>("ApplicationConfig")
						 : new ApplicationConfiguration();

			_relationshipTime = (DateTime.Now - _appConfig.MemorableDate).ToString(@"d\:hh\:mm").Split(":");

			await _userState.UpdateUser(await LocalStorageHelper.GetAuthToken(_localStorage));

			await GetCurrentUser();
			await GetSlimUsers();
		}

		protected async Task GetCurrentUser()
		{
			var userReference = _userState.GetUserReference();
			if (userReference != Guid.Empty)
				_userState.SetUser(await _usersBridge.GetUserByReference(userReference, await LocalStorageHelper.GetAuthToken(_localStorage)));

			StateHasChanged();
		}

		protected async Task GetSlimUsers()
		{
			var jwToken = await LocalStorageHelper.GetAuthToken(_localStorage);

			if (jwToken != String.Empty)
			{
				_users = await _usersBridge.GetSlimUsers(jwToken);
				_usersLoaded = true;
				StateHasChanged();
			}
		}
	}
}
