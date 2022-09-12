using ARYCA_Tests.Helpers;
using ARYCA_Tests.Helpers.TypeHelpers;
using Common.Classes;
using Common.Data;
using Common.DTO.Logs;
using Common.Entities.Logs;
using Common.Entities.Users;
using NUnit.Framework;
using Services.Routes;

namespace ARYCA_Tests.Services.Routes.Logs
{
	[TestFixture]
	public sealed class GivenARequestWithALogThatAlreadyExists
	{
		private LogsService _logService;
		private DataContext _dataContext;
		private IServicesResponse _subject;
		private Log _log;
		private Log _existingLog;

		private User _user;
		private CreateLogRequest _request;

		[OneTimeSetUp]
		public void Setup()
		{
			_user = UserHelper.GetActiveUser();
			_existingLog = LogHelper.GetExistingLog();
			_request = new CreateLogRequest
			{
				Name = "Name",
				Area = "Area",
				Description = "Description",
				Severity = "Warning",
				Details = "Details"
			};

			using (_dataContext = DatabaseHelper.GetDatabase())
			{
				_dataContext = DatabaseHelper.SeedUsers(_dataContext, _user);
				_dataContext = DatabaseHelper.SeedLogs(_dataContext, _existingLog);

				_logService = new LogsService(_dataContext);

				_subject = _logService.Add(_request);
				_log = (Log)_subject.Results;
			}
		}

		[Test]
		public void ThenTheResponseDoesNotHaveAnError()
		{
			Assert.That(_subject.HasError, Is.False);
		}

		[Test]
		public void ThenTheReturnedLogHasBeenUpdated()
		{
			Assert.Multiple(() =>
			{
				Assert.That(_log.Occurances == _existingLog.Occurances++);
				Assert.That(_log.FirstSeen == _existingLog.FirstSeen);
				Assert.That(_log.LastSeen == DateTime.Now.ToShortDateString());
			});
		}
	}
}
