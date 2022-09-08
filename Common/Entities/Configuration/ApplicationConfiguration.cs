using System.Runtime.Serialization;

namespace Common.Entities.Configuration
{
	[DataContract]
	public class ApplicationConfiguration
	{
		[DataMember(EmitDefaultValue = false)]
		public int Id { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string ApplicationName { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public DateTime MemorableDate { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string ImageText { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string LoginImageUrl { get; set; }

		[DataMember(EmitDefaultValue = true)]
		public decimal Balance { get; set; }

		public ApplicationConfiguration Default()
		{
			return new ApplicationConfiguration
			{
				ApplicationName = "ARYCA",
				ImageText = "Any Request You Could Ask",
				LoginImageUrl = "default-background.jpg",
				MemorableDate = DateTime.Now,
				Balance = 0.0M
			};
		}

		public bool IsValid()
		{
			return Id != 0 && ApplicationName != null && ImageText != null && LoginImageUrl != null;
		}
	}
}
