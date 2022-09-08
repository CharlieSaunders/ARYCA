using Microsoft.AspNetCore.Components;

namespace Client.Components
{
	public partial class Footer
	{
		[Parameter]
		public bool ShouldDisplay { get; set; }
	}
}
