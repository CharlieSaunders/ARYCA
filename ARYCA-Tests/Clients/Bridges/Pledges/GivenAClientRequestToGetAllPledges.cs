using System;
using ARYCA_Tests.Helpers;
using ARYCA_Tests.Helpers.TypeHelpers;
using Client.ServicesBridge;
using Client.States.Toast;
using Common.Classes;
using Common.Entities.Logs;
using Common.Entities.Pledges;
using Common.HttpClients;
using Moq;
using NUnit.Framework;

namespace ARYCA_Tests.Clients.Bridges.Pledges
{
	[TestFixture]
	public sealed class GivenAClientRequestToGetAllPledges
	{
		private List<Pledge> _subject;
		private List<Pledge> _pledges;
		private PledgesBridge _pledgesBridge;

		[OneTimeSetUp]
		public async Task Setup()
		{
			_pledges = PledgeHelper.GetPledgesList();

			var user = UserHelper.GetActiveUser();
			var httpClientResponse = HttpClientResponseHelper.For(new IServicesResponse(_pledges));
			var httpClient = new Mock<IGenericHttpClient>();

			httpClient.Setup(x => x.GetAsyncConvertResult(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(httpClientResponse);

			_pledgesBridge = new PledgesBridge(httpClient.Object, new ToasterService());
			_subject = await _pledgesBridge.GetAll(user.Username);
		}

		[Test]
		public void ThenThPledgesReturnedAreNotNull()
		{
			Assert.That(_subject, Is.Not.Null);
		}

		[Test]
		public void ThenThePledgesReturnedMatch()
		{
			Assert.That(_subject.Count, Is.EqualTo(_pledges.Count));
		}
	}
}

