using System.Runtime.Serialization;

namespace Common.Entities.Logs
{
	[DataContract]
	public class Log
	{
		[DataMember(EmitDefaultValue = false)]
		public int Id { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Area { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Details { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Severity { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string FirstSeen { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string LastSeen { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public int Occurances { get; set; }

		public struct LogSeverity
		{
			public const string Info = "Information";
			public const string Warning = "Warning";
			public const string Error = "Error";
			public const string Fatal = "Fatal";
			public const string Debug = "Debug";
		}

		public bool IsEmpty()
		{
			return Id == 0 && Occurances == 0;
		}

	}
}
