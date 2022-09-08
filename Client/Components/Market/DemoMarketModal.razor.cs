using Common.DTO.Unlockables;
using Microsoft.AspNetCore.Components;

namespace Client.Components.Market
{
	public partial class DemoMarketModal
	{
		[Parameter]
		public UnlockableResponse Theme { get; set; }

		[Parameter]
		public EventCallback<string> OnClose { get; set; }

		private Task ModalCancel()
		{
			return OnClose.InvokeAsync(null);
		}

		private Task ModalOk()
		{
			return OnClose.InvokeAsync("Demo");
		}
	}
}
