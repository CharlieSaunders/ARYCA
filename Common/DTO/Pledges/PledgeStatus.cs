namespace Common.DTO.Pledges
{
	public static class PledgeStatuses
	{

		public struct PledgeStatus
		{
			public const string AwaitingAcceptance = "Pending acceptance";
			public const string DeclinedAcceptance = "Not accepted by assignee";
			public const string AwaitingCompletion = "Awaiting completion";
			public const string AwaitingSignOff = "Awaiting Sign off";
			public const string DeclinedSignOff = "Assigner Declined sign off";
			public const string SignedOff = "Signed off";
		}
	}
}
