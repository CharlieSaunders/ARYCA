using System.ComponentModel.DataAnnotations;

namespace Common.DTO.Habits
{
	public class CreateHabitRequest
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }
	}
}
