using ARYCA_Tests.Helpers;
using ARYCA_Tests.Helpers.TypeHelpers;
using Client.ServicesBridge;
using Client.States.Toast;
using Common.Classes;
using Common.Entities.Logs;
using Common.HttpClients;
using Moq;
using NUnit.Framework;

namespace ARYCA_Tests.Clients.Bridges.Logs
{
	[TestFixture]
	public sealed class GivenAClientRequestToGetAllLogs
	{
		private List<Log> _subject;
		private List<Log> _logs;
		private LogsBridge _logsBridge;

		[OneTimeSetUp]
		public async Task Setup()
		{
			_logs = LogHelper.GetExistingLogList();

			var user = UserHelper.GetActiveUser();
			var httpClientResponse = HttpClientResponseHelper.For(new IServicesResponse(_logs));
			var httpClient = new Mock<IGenericHttpClient>();

			httpClient.Setup(x => x.GetAsyncConvertResult(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(httpClientResponse);

			_logsBridge = new LogsBridge(httpClient.Object, new ToasterService());
			_subject = await _logsBridge.GetAllLogs(user.Username);
		}

		[Test]
		public void ThenTheLogsReturnedAreNotNull()
		{
			Assert.That(_subject, Is.Not.Null);
		}

		[Test]
		public void ThenTheLogsReturnedMatch()
		{
			Assert.That(_subject, Has.Count.EqualTo(_logs.Count));
		}
	}
}

