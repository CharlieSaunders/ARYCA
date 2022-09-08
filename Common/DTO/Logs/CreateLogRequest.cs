using Common.Entities.Logs;
using System.ComponentModel.DataAnnotations;

namespace Common.DTO.Logs
{
	public class CreateLogRequest
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public string Severity { get; set; }

		[Required]
		public string Details { get; set; }

		[Required]
		public string Area { get; set; }

		public CreateLogRequest()
		{

		}

		public CreateLogRequest(Exception ex, string logLevel, string name, Type area)
		{
			Name = name;
			Description = ex.Message;
			Area = area.ToString();
			Severity = logLevel;
			Details = $"{ex.InnerException}";
		}

		public CreateLogRequest(string throwingClass, string details)
		{
			Name = $"Unhandled error in {throwingClass}";
			Description = "An unhandled exception that did not throw an error occurred";
			Area = throwingClass;
			Severity = Log.LogSeverity.Error;
			Details = details;
		}
	}
}
