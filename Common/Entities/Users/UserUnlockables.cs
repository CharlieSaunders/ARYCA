using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Common.Entities.Users
{
	[DataContract]
	public class UserUnlockables
	{

		[Key]
		[DataMember]
		public int Id { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public Guid UserReference { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public Guid UnlockableReference { get; set; }

		public bool IsEmpty()
		{
			return Id == 0 && UserReference == Guid.Empty && UnlockableReference == Guid.Empty;
		}
	}
}
