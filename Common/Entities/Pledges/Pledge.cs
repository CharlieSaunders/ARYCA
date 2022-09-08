using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Common.Entities.Pledges
{
	[DataContract]
	public class Pledge
	{
		[Key]
		[DataMember(EmitDefaultValue = false)]
		public Guid Reference { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Details { get; set; }

		public bool IsEmpty()
		{
			return Reference == Guid.Empty;
		}
	}
}
