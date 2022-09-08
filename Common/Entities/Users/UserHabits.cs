using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Common.Entities.Users
{
	[DataContract]
	public class UserHabits
	{
		[Key]
		[DataMember(EmitDefaultValue = false)]
		public int Id { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public Guid UserReference { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public Guid HabitReference { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public DateTime LastCompleted { get; set; }

		public bool CanBeCompleted()
		{
			var midnightToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			return LastCompleted < midnightToday;
		}
	}
}
