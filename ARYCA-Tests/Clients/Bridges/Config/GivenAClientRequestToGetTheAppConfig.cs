using System;
using ARYCA_Tests.Helpers;
using ARYCA_Tests.Helpers.TypeHelpers;
using Client.ServicesBridge;
using Client.States.Toast;
using Common.Classes;
using Common.DTO.Achievements;
using Common.Entities.Configuration;
using Common.HttpClients;
using Moq;
using NUnit.Framework;

namespace ARYCA_Tests.Clients.Bridges.Config
{
	[TestFixture]
	public sealed class GivenAClientRequestToGetTheAppConfig
	{
		private ApplicationConfiguration _subject;
		private ApplicationConfiguration _appConfig;
		private ConfigManagerBridge _configManagerBridge;

		[OneTimeSetUp]
		public async Task Setup()
		{
			_appConfig = new ApplicationConfiguration().Default();

			var user = UserHelper.GetActiveUser();
			var httpClientResponse = HttpClientResponseHelper.For(new IServicesResponse(_appConfig));
			var httpClient = new Mock<IGenericHttpClient>();

			httpClient.Setup(x => x.GetAsyncConvertResult(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(httpClientResponse);

			_configManagerBridge = new ConfigManagerBridge(httpClient.Object, new ToasterService());
			_subject = await _configManagerBridge.GetAppConfig(user.Username);
		}

		[Test]
		public void ThenTheConfigIsNotNull()
		{
			Assert.That(_subject, Is.Not.Null);
		}

		[Test]
		public void ThenTheAppConfigReturnedMatches()
		{
			Assert.Multiple(() =>
			{
				Assert.That(_subject.ImageText, Is.EqualTo(_appConfig.ImageText));
				Assert.That(_subject.MemorableDate, Is.EqualTo(_appConfig.MemorableDate));
				Assert.That(_subject.LoginImageUrl, Is.EqualTo(_appConfig.LoginImageUrl));
				Assert.That(_subject.ApplicationName, Is.EqualTo(_appConfig.ApplicationName));
			});
		}
	}
}

