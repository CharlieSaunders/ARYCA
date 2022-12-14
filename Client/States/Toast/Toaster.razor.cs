using Client.States.Toast.Types;
using Microsoft.AspNetCore.Components;

namespace Client.States.Toast
{
#pragma warning disable S3881 // "IDisposable" should be implemented correctly
	public partial class Toaster : ComponentBase, IDisposable
	{
		[Inject] private ToasterService? _toasterService { get; set; }

		private ToasterService toasterService => _toasterService!;

		protected override void OnInitialized()
		{
			toasterService.ToasterChanged += ToastChanged;
			toasterService.ToasterTimerElapsed += ToastChanged;
		}

		private void ClearToast(ToastableObject toast)
			=> toasterService.ClearToast(toast);

		private void ToastChanged(object? sender, EventArgs e)
			=> InvokeAsync(StateHasChanged);

		public void Dispose()
		{
			toasterService.ToasterChanged -= ToastChanged;
			toasterService.ToasterTimerElapsed -= ToastChanged;
			GC.SuppressFinalize(this);
		}

		private static string ToastStyle(ToastableObject toast)
		{
			var colour = Enum.GetName(typeof(MessageColour), toast.MessageColour)?.ToLower();
			return toast.MessageColour switch
			{
				MessageColour.Light => "bg-light",
				_ => $"bg-{colour} text-white"
			};
		}
	}
#pragma warning restore S3881
}


