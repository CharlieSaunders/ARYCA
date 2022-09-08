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
	public sealed class GivenARequestToSellAStock
	{
		private IServicesResponse _apiResponse;
		private StocksController _stocksController;
		private DataContext _dataContext;

		private User _user, _userSubject;
		private StockSellRequest _request;
		private UserInvestments _investment, _userInvestmentSubject;

		[OneTimeSetUp]
		public void Setup()
		{
			_user = UserHelper.GetActiveUser();
			_investment = InvestmentHelper.GetGenericInvestment();

			using (_dataContext = DatabaseHelper.GetDatabase())
			{
				_dataContext = DatabaseHelper.SeedUsers(_dataContext, _user);
				_dataContext = DatabaseHelper.SeedUserInvestments(_dataContext, _investment);

				_request = new StockSellRequest
				{
					Id = _investment.Id,
					SellPrice = 5,
					Shares = _investment.Share,
					Symbol = _investment.Symbol
				};


				_stocksController = ControllerHelper.GetStocksController(_dataContext, HttpContextHelper.GetWithArycaUserReference(_user.UserReference.ToString()));

				var subject = _stocksController.SellStock(_request).Result as ObjectResult;
				_apiResponse = (IServicesResponse)subject.Value;

				_userInvestmentSubject = _dataContext.UserInvestments.FirstOrDefault(x => x.UserReference == _user.UserReference && x.Id == _investment.Id);
				_userSubject = _dataContext.Users.FirstOrDefault(x => x.UserReference == _user.UserReference);
			}

		}

		[Test]
		public void ThenTheResponseDoesNotHaveAnError()
		{
			Assert.That(_apiResponse.HasError, Is.False);
		}

		[Test]
		public void ThenTheStockWasRemovedFromTheUserTable()
		{
			Assert.That(_userInvestmentSubject, Is.Null);
		}

		[Test]
		public void ThenTheUserBalanceWasUpdated()
		{
			Assert.That(_userSubject.Balance, Is.EqualTo(600M));
		}

	}
}
