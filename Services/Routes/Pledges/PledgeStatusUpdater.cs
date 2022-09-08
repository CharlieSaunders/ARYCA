using Common.Entities.Users;
using static Common.DTO.Pledges.PledgeStatuses;

namespace Services.Routes.Pledges
{
	public static class PledgeStatusUpdater
	{
		public static UserPledges From(UserPledges pledge, string newStatus)
		{
			if (newStatus == PledgeStatus.AwaitingAcceptance)
			{
				pledge.AssigneeAccepted = false;
				pledge.AssigneeCompleted = false;
				pledge.AssignerSignedOff = false;
				return pledge;
			}

			if (newStatus == PledgeStatus.AwaitingCompletion || newStatus == PledgeStatus.DeclinedSignOff)
			{
				pledge.AssigneeAccepted = true;
				pledge.AssigneeCompleted = false;
				pledge.AssignerSignedOff = false;
				return pledge;
			}

			if (newStatus == PledgeStatus.AwaitingSignOff)
			{
				pledge.AssigneeAccepted = true;
				pledge.AssigneeCompleted = true;
				pledge.AssignerSignedOff = false;
				return pledge;
			}

			return pledge;
		}

		public static bool ShouldUpdate(string status)
		{
			if (status == PledgeStatus.DeclinedAcceptance || status == PledgeStatus.SignedOff)
				return false;

			return true;
		}
	}
}
