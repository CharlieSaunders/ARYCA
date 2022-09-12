using System;
using ARYCA_Tests.Helpers;
using ARYCA_Tests.Helpers.TypeHelpers;
using Client.ServicesBridge;
using Client.States.Toast;
using Common.Classes;
using Common.DTO.Pledges;
using Common.Entities.Pledges;
using Common.HttpClients;
using Moq;
using NUnit.Framework;

namespace ARYCA_Tests.Clients.Bridges.Pledges
{
	[TestFixture]
	public sealed class GivenAClientRequestToGetAssignedPledges
	{
		private List<AssignedPledgeResponse> _subject;
		private List<AssignedPledgeResponse> _pledges;
		private PledgesBridge _pledgesBridge;

		[OneTimeSetUp]
		public async Task Setup()
		{
			_pledges = PledgeHelper.GetAssignedList();

			var user = UserHelper.GetActiveUser();
			var httpClientResponse = HttpClientResponseHelper.For(new IServicesResponse(_pledges));
			var httpClient = new Mock<IGenericHttpClient>();

			httpClient.Setup(x => x.GetAsyncConvertResult(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(httpClientResponse);

			_pledgesBridge = new PledgesBridge(httpClient.Object, new ToasterService());
			_subject = await _pledgesBridge.GetAssigned(user.Username);
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

