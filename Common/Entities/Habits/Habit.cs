using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Common.Entities.Habits
{
	[DataContract]
	public class Habit
	{

		[Key]
		[DataMember]
		public Guid Reference { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public decimal Value { get; set; }

		[DataMember(EmitDefaultValue = false)]
		[NotMapped]
		public DateTime LastCompleted { get; set; }

		[DataMember(EmitDefaultValue = false)]
		[NotMapped]
		public bool Completable { get; set; }

		public bool CanBeCompleted()
		{
			var midnightToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			return LastCompleted < midnightToday;
		}
	}
}
