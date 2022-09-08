using Common.Entities.Users;

namespace Common.DTO.Pledges
{
	public class RepositoryAddPledgeRequest
	{
		public Guid AssignerReference { get; set; }
		public Guid AssigneeReference { get; set; }
		public Guid PledgeReference { get; set; }
		public string AdditionalDetails { get; set; }
		public decimal Value { get; set; }

		public RepositoryAddPledgeRequest()
		{

		}

		public RepositoryAddPledgeRequest(User user, User otherUser, AssignPledgeRequest request)
		{
			AssignerReference = user.UserReference;
			AssigneeReference = otherUser.UserReference;
			PledgeReference = request.PledgeReference;
			AdditionalDetails = request.AdditionalInformation;
			Value = request.Value;
		}
	}
}
