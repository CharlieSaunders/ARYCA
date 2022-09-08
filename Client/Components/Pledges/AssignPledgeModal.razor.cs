using Common.DTO.Pledges;
using Common.Entities.Pledges;
using Microsoft.AspNetCore.Components;

namespace Client.Components.Pledges
{
	public partial class AssignPledgeModal
	{
		[Parameter]
		public Pledge SelectedPledge { get; set; }

		[Parameter]
		public AssignPledgeRequest Request { get; set; }

		[Parameter]
		public EventCallback<AssignPledgeRequest> OnClose { get; set; }

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
			Request.PledgeReference = SelectedPledge.Reference;
			return OnClose.InvokeAsync(Request);
		}
	}
}
