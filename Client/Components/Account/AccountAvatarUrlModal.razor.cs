using Common.DTO.Unlockables;
using Microsoft.AspNetCore.Components;

namespace Client.Components.Account
{
	public partial class AccountAvatarUrlModal
	{
		[Parameter]
		public string AvatarUrl { get; set; }
		[Parameter]
		public EventCallback<string> OnClose { get; set; }

		[Parameter]
		public ModalType DisplayType { get; set; }

		[Parameter]
		public string Theme { get; set; }

		[Parameter]
		public List<UnlockableResponse> UnlockedAvatars { get; set; }

		private Task ModalCancel()
		{
			return OnClose.InvokeAsync(null);
		}

		private Task SetUrl(UnlockableResponse avatar)
		{
			return OnClose.InvokeAsync(avatar.Value);
		}
	}
}
