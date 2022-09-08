using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Common.Entities.Users
{
	[DataContract]
	public class UserPledges
	{
		[Key]
		[DataMember(EmitDefaultValue = false)]
		public int Id { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public Guid AssignerReference { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public Guid AssigneeReference { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public Guid PledgeReference { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public decimal Value { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string AdditionalInformation { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public bool AssigneeCompleted { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public bool AssignerSignedOff { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public bool AssigneeAccepted { get; set; }

		public bool IsEmpty()
		{
			return AssignerReference == Guid.Empty && AssigneeReference == Guid.Empty && PledgeReference == Guid.Empty;
		}

	}
}
