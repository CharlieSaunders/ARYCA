using ARYCA_Tests.Helpers;
using ARYCA_Tests.Helpers.TypeHelpers;
using Client.ServicesBridge;
using Client.States.Toast;
using Common.Classes;
using Common.Entities.Investments;
using Common.HttpClients;
using Moq;
using NUnit.Framework;

namespace ARYCA_Tests.Clients.Bridges.Stocks
{
	[TestFixture]
	public sealed class GivenAClientRequestToGetUserInvestments
	{
		private List<UserInvestments> _subject;
		private List<UserInvestments> _userInvestments;
		private StocksBridge _stocksBridge;

		[OneTimeSetUp]
		public async Task Setup()
		{
			_userInvestments = InvestmentHelper.GetGenericInvestmentsList();

			var user = UserHelper.GetActiveUser();
			var httpClientResponse = HttpClientResponseHelper.For(new IServicesResponse(_userInvestments));
			var httpClient = new Mock<IGenericHttpClient>();

			httpClient.Setup(x => x.GetAsyncConvertResult(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(httpClientResponse);

			_stocksBridge = new StocksBridge(httpClient.Object, new ToasterService());
			_subject = await _stocksBridge.GetUserCrypto(user.Username);
		}

		[Test]
		public void ThenTheInvestmentsReturnedAreNotNull()
		{
			Assert.That(_subject, Is.Not.Null);
		}

		[Test]
		public void ThenTheInvestmentsReturnedMatch()
		{
			Assert.That(_subject, Has.Count.EqualTo(_userInvestments.Count));
		}
	}
}

