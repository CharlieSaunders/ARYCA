using System.ComponentModel.DataAnnotations;

namespace Common.DTO.Pledges
{
	public class CreatePledgeRequest
	{
		[Required(ErrorMessage = "Name is required to create a Pledge", AllowEmptyStrings = false)]
		public string Name { get; set; }

		[Required(ErrorMessage = "Details required to create a Pledge", AllowEmptyStrings = false)]
		public string Details { get; set; }
	}
}
