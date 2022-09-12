using System;
using ARYCA_Tests.Helpers;
using ARYCA_Tests.Helpers.TypeHelpers;
using Client.ServicesBridge;
using Client.States.Toast;
using Common.Classes;
using Common.DTO.Unlockables;
using Common.Entities.Users;
using Common.HttpClients;
using Moq;
using NUnit.Framework;

namespace ARYCA_Tests.Clients.Bridges.Unlockables
{
	[TestFixture]
	public sealed class GivenAClientRequestToGetAllUnlockables
	{
		private List<UnlockableResponse> _subject;
		private List<UnlockableResponse> _userUnlockables;
		private UnlockablesBridge _unlockablesBridge;

		[OneTimeSetUp]
		public async Task Setup()
		{
			_userUnlockables = UnlockablesHelper.GetUnlockableResponseList();

			var user = UserHelper.GetActiveUser();
			var httpClientResponse = HttpClientResponseHelper.For(new IServicesResponse(_userUnlockables));
			var httpClient = new Mock<IGenericHttpClient>();

			httpClient.Setup(x => x.GetAsyncConvertResult(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(httpClientResponse);

			_unlockablesBridge = new UnlockablesBridge(httpClient.Object, new ToasterService());
			_subject = await _unlockablesBridge.GetAll(user.Username);
		}

		[Test]
		public void ThenTheUnlockablesReturnedAreNotNull()
		{
			Assert.That(_subject, Is.Not.Null);
		}

		[Test]
		public void ThenTheUnlockablesReturnedMatch()
		{
			Assert.That(_subject.Count, Is.EqualTo(_userUnlockables.Count));
		}
	}
}

