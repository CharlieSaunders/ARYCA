using ARYCA_Tests.Helpers;
using Common.Classes;
using Common.Data;
using Common.Entities.Logs;
using Common.Entities.Users;
using NUnit.Framework;
using Services.Routes;

namespace ARYCA_Tests.Services.Routes.Logs
{
	[TestFixture]
	public sealed class GivenARequestToDeleteALog
	{
		private LogsService _logService;
		private DataContext _dataContext;
		private IServicesResponse _subject;
		private bool _log;
		private Log _existingLog;
		private Log _logSubject;

		private User _user;

		[OneTimeSetUp]
		public void Setup()
		{
			_user = UserHelper.GetActiveUser();
			_existingLog = LogHelper.GetExistingLog();

			using (_dataContext = DatabaseHelper.GetDatabase())
			{
				_dataContext = DatabaseHelper.SeedUsers(_dataContext, _user);
				_dataContext = DatabaseHelper.SeedLogs(_dataContext, _existingLog);

				_logService = new LogsService(_dataContext);

				_subject = _logService.Delete(_existingLog.Id);
				_log = (bool)_subject.Results;
				_logSubject = _dataContext.Logs.FirstOrDefault(x => x.Id == _existingLog.Id);
			}
		}

		[Test]
		public void ThenTheResponseDoesNotHaveAnError()
		{
			Assert.That(_subject.HasError, Is.False);
		}

		[Test]
		public void ThenTheCorrectResultIsReturned()
		{
			Assert.That(_log, Is.True);
		}

		[Test]
		public void ThenTheLogHasBeenDeleted()
		{
			Assert.That(_logSubject, Is.Null);
		}
	}
}
