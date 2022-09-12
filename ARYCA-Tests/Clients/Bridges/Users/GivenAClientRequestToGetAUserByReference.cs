using System;
using ARYCA_Tests.Helpers;
using ARYCA_Tests.Helpers.TypeHelpers;
using Client.ServicesBridge;
using Client.States.Toast;
using Common.Classes;
using Common.Classes.Users;
using Common.Entities.Users;
using Common.HttpClients;
using Moq;
using NUnit.Framework;

namespace ARYCA_Tests.Clients.Bridges.Users
{
	[TestFixture]
	public sealed class GivenAClientRequestToGetAUserByReference
	{
		private UserResponse _subject;
		private UserResponse _user;
		private UsersBridge _usersBridge;

		[OneTimeSetUp]
		public async Task Setup()
		{
			_user = UserHelper.GetUserResponse();
			var httpClientResponse = HttpClientResponseHelper.For(new IServicesResponse(_user));
			var httpClient = new Mock<IGenericHttpClient>();

			httpClient.Setup(x => x.GetAsyncConvertResult(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(httpClientResponse);

			_usersBridge = new UsersBridge(httpClient.Object, new ToasterService());
			_subject = await _usersBridge.GetUserByReference(_user.UserReference, _user.Username);
		}

		[Test]
		public void ThenTheUserReturnedAreNotNull()
		{
			Assert.That(_subject, Is.Not.Null);
		}

		[Test]
		public void ThenTheUserIsNotMutated()
		{
			Assert.Multiple(() =>
			{
				Assert.That(_subject.Username, Is.EqualTo(_user.Username));
				Assert.That(_subject.UserReference, Is.EqualTo(_user.UserReference));
			});
		}
	}
}

