using System.Runtime.Serialization;

namespace Common.Entities.Investments
{
	[DataContract]
	public class UserInvestments
	{
		[DataMember(EmitDefaultValue = false)]
		public int Id { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public Guid UserReference { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Symbol { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public decimal Share { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public decimal Price { get; set; }

		public bool IsEmpty()
		{
			return Id == 0 && UserReference == Guid.Empty;
		}

		public decimal TotalCost() => Share * Price;
	}
}
