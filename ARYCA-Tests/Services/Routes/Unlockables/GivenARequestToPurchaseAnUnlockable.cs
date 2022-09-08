using ARYCA_Tests.Helpers;
using Common.Classes;
using Common.Data;
using Common.Entities.Unlockables;
using Common.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Services.Controllers;

namespace ARYCA_Tests.Services.Routes.Unlockables
{
	[TestFixture]
	public sealed class GivenARequestToPurchaseAnUnlockable
	{
		private IServicesResponse _apiResponse;
		private UnlockablesController _stocksController;
		private DataContext _dataContext;

		private User _user;
		private User _userSubject;
		private Unlockable _unlockable;
		private UserUnlockables _unlockableSubject;
		private UserTransaction _transactionSubject;

		[OneTimeSetUp]
		public void Setup()
		{
			_user = UserHelper.GetActiveUser();
			_unlockable = UnlockablesHelper.GetGenericUnlockable();

			using (_dataContext = DatabaseHelper.GetDatabase())
			{
				_dataContext = DatabaseHelper.SeedUsers(_dataContext, _user);
				_dataContext = DatabaseHelper.SeedUnlockables(_dataContext, _unlockable);

				_stocksController = ControllerHelper.GetUnlockablesController(
					_dataContext,
					HttpContextHelper.GetWithArycaUserReference(_user.UserReference.ToString()));

				var subject = _stocksController.Unlock(_unlockable.Reference).Result as ObjectResult;
				_apiResponse = (IServicesResponse)subject.Value;

				_unlockableSubject = _dataContext.UserUnlockables.FirstOrDefault(x => x.UnlockableReference == _unlockable.Reference && x.UserReference == _user.UserReference);
				_userSubject = _dataContext.Users.FirstOrDefault(x => x.UserReference == _user.UserReference);
				_transactionSubject = _dataContext.UserTransactions.FirstOrDefault(x => x.UserReference == _user.UserReference && x.Value == -10);
			}
		}

		[Test]
		public void ThenTheResponseDoesNotHaveAnError()
		{
			Assert.That(_apiResponse.HasError, Is.False);
		}

		[Test]
		public void ThenTheUserHasUnlockedThisUnlockable()
		{
			Assert.That(_unlockableSubject, Is.Not.Null);
		}

		[Test]
		public void ThenTheUserBalanceHasBeenUpdated()
		{
			Assert.That(_userSubject.Balance, Is.EqualTo(90));
		}

		[Test]
		public void ThenTheTransactionHasBeenLogged()
		{
			Assert.That(_transactionSubject, Is.Not.Null);
		}
	}
}
