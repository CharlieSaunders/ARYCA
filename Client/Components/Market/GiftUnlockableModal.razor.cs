using Common.DTO.Unlockables;
using Microsoft.AspNetCore.Components;

namespace Client.Components.Market
{
	public partial class GiftUnlockableModal
	{
		[Parameter]
		public UnlockableResponse Unlockable { get; set; }

		[Parameter]
		public EventCallback<GiftUnlockableRequest> OnClose { get; set; }

		[Parameter]
		public string Theme { get; set; }

		private GiftUnlockableRequest _giftUnlockableRequest = new();

		private Task ModalCancel()
		{
			return OnClose.InvokeAsync(null);
		}

		private Task ModalOk()
		{
			_giftUnlockableRequest.UnlockableReference = Unlockable.UnlockableReference;
			return OnClose.InvokeAsync(_giftUnlockableRequest);
		}
	}
}
