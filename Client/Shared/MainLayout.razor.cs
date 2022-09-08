namespace Client.Shared
{
	public partial class MainLayout
	{
		private string _userTheme;

		protected override void OnInitialized()
		{
			_userTheme = _userState.GetUserTheme();
			StateHasChanged();
		}
	}
}
