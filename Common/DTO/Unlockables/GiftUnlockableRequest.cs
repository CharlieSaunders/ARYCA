using System.ComponentModel.DataAnnotations;

namespace Common.DTO.Unlockables
{
	public class GiftUnlockableRequest
	{
		[Required]
		public string GifteeReference { get; set; }

		[Required]
		public Guid UnlockableReference { get; set; }
	}
}
