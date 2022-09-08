using Common.DTO.Pledges;
using Microsoft.AspNetCore.Components;
using static Common.DTO.Pledges.PledgeStatuses;

namespace Client.Components.Pledges
{
	public partial class ViewAssignedPledgeModal
	{
		[Parameter]
		public AssignedPledgeResponse SelectedPledge { get; set; }

		[Parameter]
		public EventCallback<UpdatePledgeStatusRequest> OnClose { get; set; }

		[Parameter]
		public ModalType DisplayType { get; set; }

		[Parameter]
		public string Theme { get; set; }

		private UpdatePledgeStatusRequest updatePledgeRequest { get; set; } = new();


		private Task ModalCancel()
		{
			return OnClose.InvokeAsync(null);
		}

		private Task ModalOk()
		{
			return OnClose.InvokeAsync(updatePledgeRequest);
		}

		private Task ModalAccept()
		{
			updatePledgeRequest.PledgeId = SelectedPledge.PledgeId;
			updatePledgeRequest.NewStatus = PledgeStatus.AwaitingCompletion;
			return OnClose.InvokeAsync(updatePledgeRequest);
		}

		private Task ModalDecline()
		{
			updatePledgeRequest.PledgeId = SelectedPledge.PledgeId;
			updatePledgeRequest.NewStatus = PledgeStatus.DeclinedAcceptance;
			return OnClose.InvokeAsync(updatePledgeRequest);
		}

		private Task ModalComplete()
		{
			updatePledgeRequest.PledgeId = SelectedPledge.PledgeId;
			updatePledgeRequest.NewStatus = PledgeStatus.AwaitingSignOff;
			return OnClose.InvokeAsync(updatePledgeRequest);
		}

		private Task ModalSignOff()
		{
			updatePledgeRequest.PledgeId = SelectedPledge.PledgeId;
			updatePledgeRequest.NewStatus = PledgeStatus.SignedOff;
			return OnClose.InvokeAsync(updatePledgeRequest);
		}

		private Task ModalRejectSignOff()
		{
			updatePledgeRequest.PledgeId = SelectedPledge.PledgeId;
			updatePledgeRequest.NewStatus = PledgeStatus.AwaitingCompletion;
			return OnClose.InvokeAsync(updatePledgeRequest);
		}
	}
}
