using System.ComponentModel.DataAnnotations;

namespace Common.DTO.Habits
{
	public class UpdateHabitRequest
	{
		[Required]
		public Guid HabitReference { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }
	}
}
