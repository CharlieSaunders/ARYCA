using Common.Classes.Users;
using static Common.DTO.Pledges.PledgeStatuses;

namespace Common.DTO.Pledges
{
	public class AssignedPledgeResponse
	{
		public int PledgeId { get; set; }
		public string Name { get; set; }
		public string Details { get; set; }
		public decimal Value { get; set; }
		public SlimUser Assignee { get; set; }
		public SlimUser Assigner { get; set; }
		public string Status { get; set; }
		public bool IsWithYou { get; set; }

		public void SetStatus(bool assigneeAccepted, bool assigneeCompleted, bool assignerSignedOff)
		{
			if (!assigneeAccepted)
			{
				Status = PledgeStatus.AwaitingAcceptance;
			}
			else if (!assigneeCompleted)
			{
				Status = PledgeStatus.AwaitingCompletion;
			}
			else if (!assignerSignedOff)
			{
				Status = PledgeStatus.AwaitingSignOff;
			}
		}

		public void MarkCurrentOwner(Guid userRef)
		{
			if (Assigner.Reference == userRef)
			{
				if (Status == PledgeStatus.AwaitingSignOff)
					IsWithYou = true;
			}
			else
			{
				if (Status != PledgeStatus.AwaitingSignOff)
					IsWithYou = true;
			}
		}
	}
}
