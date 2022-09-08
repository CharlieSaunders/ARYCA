namespace Client.Shared
{
	public partial class NavMenu
	{
		private bool collapseNavMenu = true;
		private bool expandSubNav = false;
		private string _userTheme;
		private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

		protected override void OnInitialized()
		{
			_userTheme = _userState.GetUserTheme();
			StateHasChanged();
		}

		private void ToggleNavMenu()
		{
			collapseNavMenu = !collapseNavMenu;
		}

		private async Task Logout()
		{
			_userState.Logout();
			await _localStorage.RemoveItemsAsync(new List<string> { "AuthenticationToken", "AutomaticLogin", "LastPage", "UserReference", "Username" });
			_navigationManager.NavigateTo("Login");
			await _authStateProvider.GetAuthenticationStateAsync();
		}
	}
}
