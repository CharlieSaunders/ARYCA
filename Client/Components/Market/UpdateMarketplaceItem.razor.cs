using Common.DTO.Unlockables;
using Microsoft.AspNetCore.Components;

namespace Client.Components.Market
{
	public partial class UpdateMarketplaceItem
	{
		[Parameter]
		public UnlockableResponse SelectedItem { get; set; }

		[Parameter]
		public string Theme { get; set; }

		[Parameter]
		public ModalType DisplayType { get; set; }

		[Parameter]
		public EventCallback<UnlockableResponse> OnClose { get; set; }

		private CreateUnlockableRequest _newItemRequest = new CreateUnlockableRequest();

		private Task ModalCancel()
		{
			return OnClose.InvokeAsync(null);
		}

		private Task ModalOk()
		{
			return OnClose.InvokeAsync(SelectedItem);
		}
	}
}
