
using System.ComponentModel.DataAnnotations;

namespace Common.DTO.Unlockables
{
	public class CreateUnlockableRequest
	{
		[Required(ErrorMessage = "Name is required when creating an unlockable", AllowEmptyStrings = false)]
		public string Name { get; set; }

		[Required(ErrorMessage = "Type is required when creating an unlockable", AllowEmptyStrings = false)]
		public string Type { get; set; }

		[Required(ErrorMessage = "Price is required when creating an unlockable")]
		public decimal Price { get; set; }

		[Required(ErrorMessage = "Value is required when creating an unlockable", AllowEmptyStrings = false)]
		public string Value { get; set; }

		[Required(ErrorMessage = "Display is required when creating an unlockable")]
		public bool Display { get; set; }
	}
}
