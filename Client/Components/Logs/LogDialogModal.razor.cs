using Common.Entities.Logs;
using Microsoft.AspNetCore.Components;

namespace Client.Components.Logs
{
	public partial class LogDialogModal
	{
		[Parameter]
		public Log SelectedLog { get; set; }

		[Parameter]
		public EventCallback<Log> OnClose { get; set; }

		[Parameter]
		public ModalType DisplayType { get; set; }

		[Parameter]
		public string Theme { get; set; }

		private Task ModalCancel()
		{
			return OnClose.InvokeAsync(null);
		}

		private Task ModalOk()
		{
			return OnClose.InvokeAsync(null);
		}

		private Task ModalDelete()
		{
			return OnClose.InvokeAsync(SelectedLog);
		}
	}
}
