using Microsoft.AspNetCore.Components;

namespace Client.Components.Admin
{
	public partial class SelectLoginImageModal
	{
		[Parameter]
		public string Current { get; set; }

		[Parameter]
		public EventCallback<string> OnClose { get; set; }

		[Parameter]
		public string Theme { get; set; }

		private List<string> Images = new List<string>()
		{
			"amsterdam.jpg", "CT_CS_wedding.jpg", "default-background.jpg", "lanzarote.jpg", "peckforton.jpg", "astronaut.jpg"
		};

		private Task ModalCancel()
		{
			return OnClose.InvokeAsync(null);
		}

		private Task SetUrl(string imageUrl)
		{
			return OnClose.InvokeAsync(imageUrl);
		}
	}
}
