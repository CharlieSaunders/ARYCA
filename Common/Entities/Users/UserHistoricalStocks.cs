using System.Runtime.Serialization;

namespace Common.Entities.Users
{
	[DataContract]
	public class UserHistoricalStocks
	{
		[DataMember(EmitDefaultValue = false)]
		public int Id { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Time { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public Guid UserReference { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Symbol { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public decimal Shares { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public decimal Profit { get; set; }
	}
}
