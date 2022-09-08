using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Common.Entities.Unlockables
{
	[DataContract]
	public class Unlockable
	{
		[DataMember]
		[Key]
		public Guid Reference { get; set; }

		[DataMember]
		public string Type { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public decimal Price { get; set; }

		[DataMember]
		public string Value { get; set; }

		[DataMember]
		public bool Display { get; set; }

		public Unlockable()
		{

		}

		public Unlockable(string type, string name, decimal price, string value, bool display)
		{
			Type = type;
			Name = name;
			Price = price;
			Value = value;
			Display = display;
		}

		public bool IsFree()
		{
			return Price == 0.0M;
		}

		public bool IsEmpty()
		{
			return Reference == Guid.Empty;
		}
	}
}
