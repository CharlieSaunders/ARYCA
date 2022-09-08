using ARYCA_Tests.Helpers;
using Common.Classes;
using Common.Data;
using Common.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Services.Controllers;

namespace ARYCA_Tests.Services.Routes.Config
{
	[TestFixture]
	public sealed class GivenARequestToGetTheAppConfig
	{
		private IServicesResponse _apiResponse;
		private ConfigController _configController;
		private DataContext _datacontext;

		private User _user;

		[OneTimeSetUp]
		public void Setup()
		{
			_user = UserHelper.GetActiveUser();

			using (_datacontext = DatabaseHelper.GetDatabase())
			{
				_datacontext = DatabaseHelper.SeedUsers(_datacontext, _user);

				_configController = ControllerHelper.GetConfigController(_datacontext,
					HttpContextHelper.GetWithArycaUserReference(_user.UserReference.ToString())
				);

				var subject = _configController.GetAppConfig().Result as ObjectResult;
				_apiResponse = (IServicesResponse)subject.Value;
			}
		}

		[Test]
		public void ThenTheResponseDoesNotHaveAnError()
		{
			Assert.That(_apiResponse.HasError, Is.False);
		}
	}
}
