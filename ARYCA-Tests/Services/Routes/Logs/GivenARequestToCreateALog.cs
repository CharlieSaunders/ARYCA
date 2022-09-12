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
	public sealed class GivenARequestToCreateALog
	{
		private LogsService _logService;
		private DataContext _dataContext;
		private IServicesResponse _subject;
		private Log _log;
		private Log _logSubject;

		private User _user;
		private CreateLogRequest _request;

		[OneTimeSetUp]
		public void Setup()
		{
			_user = UserHelper.GetActiveUser();

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

				_logService = new LogsService(_dataContext);

				_subject = _logService.Add(_request);
				_log = (Log)_subject.Results;

				_logSubject = _dataContext.Logs.FirstOrDefault(x => x.Id == _log.Id);
			}
		}

		[Test]
		public void ThenTheResponseDoesNotHaveAnError()
		{
			Assert.That(_subject.HasError, Is.False);
		}

		[Test]
		public void ThenTheLogWasCreated()
		{
			Assert.That(_logSubject, Is.Not.Null);
		}

		[Test]
		public void ThenTheLogWasCreatedCorrectly()
		{
			Assert.Multiple(() =>
			{
				Assert.That(_logSubject.Id == _log.Id);
				Assert.That(_logSubject.Name == _log.Name);
				Assert.That(_logSubject.Details == _log.Details);
				Assert.That(_logSubject.Severity == _log.Severity);
				Assert.That(_logSubject.Occurances == _log.Occurances);
				Assert.That(_logSubject.Area == _log.Area);
			});
		}
	}
}
