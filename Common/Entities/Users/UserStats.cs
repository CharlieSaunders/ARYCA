using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Common.Entities.Users
{
	[DataContract]
	public class UserStats
	{
		[Key]
		[DataMember(EmitDefaultValue = false)]
		public Guid UserReference { get; set; }

		[DataMember(EmitDefaultValue = true)]
		public int TradesMade { get; set; }

		[DataMember(EmitDefaultValue = true)]
		public int HabitsCompleted { get; set; }

		[DataMember(EmitDefaultValue = true)]
		public int PledgesCompleted { get; set; }

		[DataMember(EmitDefaultValue = true)]
		public decimal TradeProfit { get; set; }

		[DataMember(EmitDefaultValue = true)]
		public int GiftsGiven { get; set; }

		public UserStats()
		{

		}
	}
}
