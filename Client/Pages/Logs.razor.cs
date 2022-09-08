using Client.Components;
using Client.Helpers;
using Client.ServicesBridge;
using Common.Entities.Logs;

namespace Client.Pages
{
	public partial class Logs
	{
		private string _userTheme;
		private LogsBridge _logsBridge;
		private List<Log> _allLogs;

		private bool _logsLoaded = false;
		private bool _logModalDialogOpen = false;
		private Log _selectedLog;
		private ModalType _modalType;

		protected override async Task OnInitializedAsync()
		{
			await _localStorage.SetItemAsync("LastPage", "Logs");

			_userTheme = _userState.GetUserTheme();
			_logsBridge = new LogsBridge(_genericHttpClient, _toasterService);

			await _userState.UpdateUser(await LocalStorageHelper.GetAuthToken(_localStorage));

			await GetAllLogs();
		}

		protected async Task GetAllLogs()
		{
			_allLogs = await _logsBridge.GetAllLogs(await LocalStorageHelper.GetAuthToken(_localStorage));
			_logsLoaded = true;
			StateHasChanged();
		}

		private void OpenDialogModal(Log log, ModalType modalType)
		{
			_selectedLog = log;
			_modalType = modalType;
			_logModalDialogOpen = true;
			StateHasChanged();
		}

		private async void OnDialogClose(Log log)
		{
			_logModalDialogOpen = false;
			if (log is not null)
				await DeleteLog(log);
			StateHasChanged();
		}

		private async Task DeleteLog(Log log)
		{
			await _logsBridge.Delete(log.Id, await LocalStorageHelper.GetAuthToken(_localStorage));
			await GetAllLogs();
			StateHasChanged();
		}
	}
}
