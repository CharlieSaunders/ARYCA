using Client.ServicesBridge;
using Common.Entities.Configuration;
using Common.Entities.Users;

namespace Client.Pages
{
	public partial class Login
	{
		private UsersBridge _userBridge;
		private ConfigManagerBridge _configBridge;
		private User user = new User();
		private bool _displaySpinner = true;
		private string _previousLogin;

		private ApplicationConfiguration _appConfig;

		protected override async Task OnInitializedAsync()
		{
			_userBridge = new UsersBridge(_genericHttpClient, _toasterService);
			_configBridge = new ConfigManagerBridge(_genericHttpClient, _toasterService);

			_appConfig = await GetAppConfig();

			await PreviousLogin();
		}

		private async Task PreviousLogin()
		{

			user.Username = await _localStorage.ContainKeyAsync("Username") ? await _localStorage.GetItemAsStringAsync("Username") : string.Empty;
			user.UserReference = await _localStorage.ContainKeyAsync("UserReference") ? Guid.Parse(await _localStorage.GetItemAsStringAsync("UserReference")) : Guid.Empty;
			_previousLogin = await _localStorage.ContainKeyAsync("AutomaticLogin") ? await _localStorage.GetItemAsStringAsync("AutomaticLogin") : "failed";

			if (user.Username != string.Empty && !_previousLogin.Equals("failed"))
				await HandleLogin();
			else
				_displaySpinner = false;

			StateHasChanged();
		}

		private async Task<ApplicationConfiguration> GetAppConfig()
		{
			return await _localStorage.ContainKeyAsync("ApplicationConfig")
				? await _localStorage.GetItemAsync<ApplicationConfiguration>("ApplicationConfig")
				: new ApplicationConfiguration().Default();
		}

		private async Task HandleLogin()
		{
			_displaySpinner = true;
			StateHasChanged();
			if (user.Username is not null && user.Username != string.Empty)
			{
				var authUser = await _userBridge.CreateSession(user.Username.Replace("\"", ""));

				if (authUser.IsValidUser())
				{
					var appConfig = await _configBridge.GetAppConfig(authUser.AuthenticationToken);
					if (authUser.HasActiveSession)
					{
						_userState.SetUser(authUser);
						await _localStorage.SetItemAsStringAsync("AuthenticationToken", authUser.AuthenticationToken);
						await _localStorage.SetItemAsStringAsync("Username", authUser.Username);
						await _localStorage.SetItemAsStringAsync("UserReference", authUser.UserReference.ToString());
						await _localStorage.SetItemAsStringAsync("AutomaticLogin", "success");
						await _localStorage.SetItemAsync<ApplicationConfiguration>("ApplicationConfig", appConfig);
						await _authStateProvider.GetAuthenticationStateAsync();

						var mostRecentPage = await _localStorage.GetItemAsStringAsync("LastPage");

						if (mostRecentPage is not null)
							_navigationManager.NavigateTo(mostRecentPage.Replace("\"", ""));
						else
							_navigationManager.NavigateTo("/Home");
					}
					else
					{
						await _localStorage.SetItemAsStringAsync("AutomaticLogin", "failed");
						_navigationManager.NavigateTo("Login");
						_displaySpinner = false;
						StateHasChanged();
					}
				}
				else
					_displaySpinner = false;
			}

		}
	}
}
