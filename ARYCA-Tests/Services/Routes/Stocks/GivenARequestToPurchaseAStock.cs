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
	public sealed class GivenARequestToPurchaseAStock
	{
		private IServicesResponse _apiResponse;
		private StocksController _stocksController;
		private DataContext _dataContext;

		private User _user, _userSubject;
		private StockPurchaseRequest _request;
		private UserInvestments _stocksSubject;

		[OneTimeSetUp]
		public void Setup()
		{
			_user = UserHelper.GetActiveUser();

			using (_dataContext = DatabaseHelper.GetDatabase())
			{
				_dataContext = DatabaseHelper.SeedUsers(_dataContext, new List<User>() { _user });

				_request = new StockPurchaseRequest
				{
					Symbol = "StockSymbol",
					Share = 1,
					PurchasePrice = 10
				};

				_stocksController = ControllerHelper.GetStocksController(_dataContext, HttpContextHelper.GetWithArycaUserReference(_user.UserReference.ToString()));

				var subject = _stocksController.PurchaseStock(_request).Result as ObjectResult;
				_apiResponse = (IServicesResponse)subject.Value;

				_stocksSubject = _dataContext.UserInvestments.FirstOrDefault(x => x.UserReference == _user.UserReference);
				_userSubject = _dataContext.Users.FirstOrDefault(x => x.UserReference == _user.UserReference);
			}

		}

		[Test]
		public void ThenTheResponseDoesNotHaveAnError()
		{
			Assert.That(_apiResponse.HasError, Is.False);
		}

		[Test]
		public void ThenTheStockIsSavedAgainstTheUser()
		{
			Assert.Multiple(() =>
			{
				Assert.That(_stocksSubject.Symbol, Is.EqualTo(_request.Symbol));
				Assert.That(_stocksSubject.Share, Is.EqualTo(_request.Share));
				Assert.That(_stocksSubject.Price, Is.EqualTo(_request.PurchasePrice));
			});
		}

		[Test]
		public void ThenTheUserBalanceIsAdjustedForThePurchasePrice()
		{
			Assert.That(_userSubject.Balance, Is.EqualTo(90.00M));
		}
	}
}
