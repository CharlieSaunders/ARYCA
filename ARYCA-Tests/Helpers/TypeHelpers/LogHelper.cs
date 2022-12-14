using Common.Entities.Logs;

namespace ARYCA_Tests.Helpers.TypeHelpers
{
	public static class LogHelper
	{
		public static Log GetExistingLog()
		{
			return new Log
			{
				Id = 1,
				Area = "Area",
				Name = "Name",
				Description = "Description",
				Details = "Details",
				Severity = "Warning",
				FirstSeen = DateTime.Now.AddDays(-1).ToShortDateString(),
				LastSeen = DateTime.Now.AddDays(-1).ToShortDateString(),
				Occurances = 1
			};
		}

		public static List<Log> GetExistingLogList()
		{
			return new List<Log>()
			{
				GetExistingLog()
			};
		}
	}
}
