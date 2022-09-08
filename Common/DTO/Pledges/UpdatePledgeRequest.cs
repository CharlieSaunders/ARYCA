using System.ComponentModel.DataAnnotations;

namespace Common.DTO.Pledges
{
	public class UpdatePledgeRequest
	{
		[Required(ErrorMessage = "Reference required to update Pledge.")]
		public Guid PledgeReference { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string Name { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string Details { get; set; }
	}
}
