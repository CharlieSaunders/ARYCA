using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Common.Entities.Achievements
{
	[DataContract]
	public class Achievement
	{
		[Key]
		[DataMember(EmitDefaultValue = false)]
		public int Id { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public int Identifier { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Title { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string ImageUrl { get; set; }

		public Achievement()
		{

		}

		public Achievement(int id, string title, string description, string imageUrl)
		{
			Identifier = id;
			Title = title;
			Description = description;
			ImageUrl = imageUrl;
		}

		public bool IsValid()
		{
			return Identifier != 0 &&
					Title != string.Empty;
		}
	}
}
