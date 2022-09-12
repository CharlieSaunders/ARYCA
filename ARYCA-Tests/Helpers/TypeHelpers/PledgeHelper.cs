using Common.Entities.Pledges;
using Common.Entities.Users;

namespace ARYCA_Tests.Helpers.TypeHelpers
{
	public static class PledgeHelper
	{
		public static Pledge GetOldPledge()
		{
			return new Pledge
			{
				Reference = Guid.Parse("cd2271b6-de15-0010-a940-9a40a3e6106d"),
				Name = "Old Name",
				Details = "Old Pledge Details"
			};
		}

		public static UserPledges GetUserPledge(Guid assignerReference, Guid assigneeReference, Guid pledgeReference, string pledgeDetails)
		{
			return new UserPledges
			{
				AssignerReference = assignerReference,
				AssigneeReference = assigneeReference,
				PledgeReference = pledgeReference,
				Value = 1,
				AdditionalInformation = pledgeDetails,
				AssigneeAccepted = false,
				AssigneeCompleted = false,
				AssignerSignedOff = false
			};
		}
	}
}
