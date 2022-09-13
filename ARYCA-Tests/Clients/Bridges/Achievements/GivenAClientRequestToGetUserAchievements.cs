using ARYCA_Tests.Helpers;
using ARYCA_Tests.Helpers.TypeHelpers;
using Client.ServicesBridge;
using Client.States.Toast;
using Common.Classes;
using Common.DTO.Achievements;
using Common.HttpClients;
using Moq;
using NUnit.Framework;

namespace ARYCA_Tests.Clients.Bridges.Achievements
{
	[TestFixture]
	public sealed class GivenAClientRequestToGetUserAchievements
	{

		private List<UserAchievementResponse> _subject;
		private List<UserAchievementResponse> _userAchievements;
		private AchievementsBridge _achievementsBridge;

		[OneTimeSetUp]
		public async Task Setup()
		{
			_userAchievements = AchievementTestHelper.GetUserAchievements();

			var user = UserHelper.GetActiveUser();
			var httpClientResponse = HttpClientResponseHelper.For(new IServicesResponse(_userAchievements));
			var httpClient = new Mock<IGenericHttpClient>();

			httpClient.Setup(x => x.GetAsyncConvertResult(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(httpClientResponse);

			_achievementsBridge = new AchievementsBridge(httpClient.Object, new ToasterService());
			_subject = await _achievementsBridge.GetUserAchievements(user.Username);
		}

		[Test]
		public void ThenTheAchievementsReturnedAreNotNull()
		{
			Assert.That(_subject, Is.Not.Null);
		}

		[Test]
		public void ThenTheNumberOfUserAchievementMatches()
		{
			Assert.That(_subject, Has.Count.EqualTo(_userAchievements.Count));
		}
	}
}
