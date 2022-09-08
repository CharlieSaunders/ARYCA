using Common.Entities.Pledges;
using Microsoft.AspNetCore.Components;

namespace Client.Components.Pledges
{
	public partial class PledgesDialogModal
	{
		[Parameter]
		public Pledge SelectedPledge { get; set; }

		[Parameter]
		public EventCallback<Pledge> OnClose { get; set; }

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
			return OnClose.InvokeAsync(SelectedPledge);
		}
	}
}
