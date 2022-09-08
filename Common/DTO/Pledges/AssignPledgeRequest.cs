using System.ComponentModel.DataAnnotations;

namespace Common.DTO.Pledges
{
	public class AssignPledgeRequest
	{
		[Required(ErrorMessage = "Pledge reference required")]
		public Guid PledgeReference { get; set; }

		[Required(ErrorMessage = "Assigner reference required")]
		public Guid AssignerReference { get; set; }

		[Required(ErrorMessage = "Assignee username required")]
		public string AssigneeUsername { get; set; }

		[Required(ErrorMessage = "Value required")]
		public decimal Value { get; set; }

		public string AdditionalInformation { get; set; }
	}

	public class UpdatePledgeStatusRequest
	{
		public int PledgeId { get; set; }
		public string NewStatus { get; set; }
	}
}
