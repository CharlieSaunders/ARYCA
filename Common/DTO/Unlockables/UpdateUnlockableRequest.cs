using System.ComponentModel.DataAnnotations;
namespace Common.DTO.Unlockables
{
	public class UpdateUnlockableRequest
	{
		[Required]
		public Guid Reference { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public decimal Price { get; set; }

		[Required]
		public string Value { get; set; }

		[Required]
		public string Type { get; set; }

		[Required]
		public bool Display { get; set; }
	}
}
