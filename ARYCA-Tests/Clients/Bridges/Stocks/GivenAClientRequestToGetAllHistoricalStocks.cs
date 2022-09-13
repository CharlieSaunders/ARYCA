using ARYCA_Tests.Helpers;
using ARYCA_Tests.Helpers.TypeHelpers;
using Client.ServicesBridge;
using Client.States.Toast;
using Common.Classes;
using Common.Entities.Users;
using Common.HttpClients;
using Moq;
using NUnit.Framework;

namespace ARYCA_Tests.Clients.Bridges.Stocks
{
	[TestFixture]
	public sealed class GivenAClientRequestToGetAllHistoricalStocks
	{
		private List<UserHistoricalStocks> _subject;
		private List<UserHistoricalStocks> _userInvestments;
		private StocksBridge _stocksBridge;

		[OneTimeSetUp]
		public async Task Setup()
		{
			var user = UserHelper.GetActiveUser();

			_userInvestments = InvestmentHelper.GetHistoricalStocksList(user.UserReference);

			var httpClientResponse = HttpClientResponseHelper.For(new IServicesResponse(_userInvestments));
			var httpClient = new Mock<IGenericHttpClient>();

			httpClient.Setup(x => x.GetAsyncConvertResult(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(httpClientResponse);

			_stocksBridge = new StocksBridge(httpClient.Object, new ToasterService());
			_subject = await _stocksBridge.GetAllHistorical(user.Username);
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

