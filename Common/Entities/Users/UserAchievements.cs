using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Common.Entities.Users
{
	[DataContract]
	public class UserAchievements
	{
		[Key]
		[DataMember(EmitDefaultValue = false)]
		public int Id { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public Guid UserReference { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public int AchievementReference { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public int TimesUnlocked { get; set; }
	}
}
