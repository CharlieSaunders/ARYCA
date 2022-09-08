using ARYCA_Tests.Helpers;
using Common.Classes;
using Common.Data;
using Common.DTO.Stocks;
using Common.Entities.Investments;
using Common.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Services.Controllers;

namespace ARYCA_Tests.Services.Routes.Stocks
{
	[TestFixture]
	public sealed class GivenARequestToSellAPartialStock
	{
		private IServicesResponse _apiResponse;
		private StocksController _stocksController;
		private DataContext _dataContext;

		private User _user, _userSubject;
		private StockSellRequest _request;
		private UserInvestments _investment, _investmentsSubject;
		private UserTransaction _transactionsSubject;


		[OneTimeSetUp]
		public void Setup()
		{
			_user = UserHelper.GetActiveUser();
			_investment = InvestmentHelper.GetGenericInvestment();
			_investment.Id = 2;

			using (_dataContext = DatabaseHelper.GetDatabase())
			{
				_dataContext = DatabaseHelper.SeedUsers(_dataContext, _user);
				_dataContext = DatabaseHelper.SeedUserInvestments(_dataContext, _investment);

				_request = new StockSellRequest
				{
					Id = 2,
					Symbol = "Symbol",
					Shares = 1,
					SellPrice = 5
				};

				_stocksController = ControllerHelper.GetStocksController(_dataContext, HttpContextHelper.GetWithArycaUserReference(_user.UserReference.ToString()));

				var subject = _stocksController.SellStock(_request).Result as ObjectResult;
				_apiResponse = (IServicesResponse)subject.Value;

				_investmentsSubject = _dataContext.UserInvestments.FirstOrDefault(x => x.Id == _investment.Id);
				_userSubject = _dataContext.Users.FirstOrDefault(x => x.UserReference == _user.UserReference);
				_transactionsSubject = _dataContext.UserTransactions.FirstOrDefault(x => x.UserReference == _user.UserReference && x.Value == 5);
			}
		}

		[Test]
		public void ThenTheResponseDoesNotHaveAnError()
		{
			Assert.That(_apiResponse.HasError, Is.False);
		}

		[Test]
		public void ThenTheRemainingStockIsStillPresent()
		{
			Assert.That(_investmentsSubject.Share, Is.EqualTo(99));
		}

		[Test]
		public void ThenTheUserBalanceWasUpdated()
		{
			Assert.That(_userSubject.Balance, Is.EqualTo(105));
		}

		[Test]
		public void ThenAUserTransactionWasAdded()
		{
			Assert.That(_transactionsSubject, Is.Not.Null);
		}
	}
}
