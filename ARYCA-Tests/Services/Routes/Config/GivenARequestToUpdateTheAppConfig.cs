using ARYCA_Tests.Helpers;
using ARYCA_Tests.Helpers.TypeHelpers;
using Common.Classes;
using Common.Data;
using Common.DTO.Configs;
using Common.Entities.Configuration;
using Common.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Services.Controllers;

namespace ARYCA_Tests.Services.Routes.Config
{
	[TestFixture]
	public sealed class GivenARequestToUpdateTheAppConfig
	{
		private IServicesResponse _apiResponse;
		private ConfigController _configController;
		private DataContext _datacontext;

		private User _user;
		private UpdateAppConfigRequest _updateAppConfigRequest;
		private ApplicationConfiguration _updatedAppConfig;

		[OneTimeSetUp]
		public void Setup()
		{
			_user = UserHelper.GetActiveUser();
			_updateAppConfigRequest = new UpdateAppConfigRequest
			{
				AppName = "New Appname",
				Date = new DateTime(2022, 01, 01),
				ImageText = "New Imagetext",
				ImageUrl = "New ImageUrl"
			};

			using (_datacontext = DatabaseHelper.GetDatabase())
			{
				_datacontext = DatabaseHelper.SeedUsers(_datacontext, _user);

				_configController = ControllerHelper.GetConfigController(_datacontext,
					HttpContextHelper.GetWithArycaUserReference(_user.UserReference.ToString())
				);

				var subject = _configController.UpdateAppConfig(_updateAppConfigRequest).Result as ObjectResult;
				_apiResponse = (IServicesResponse)subject.Value;
				_updatedAppConfig = _apiResponse.Results;
			}
		}

		[Test]
		public void ThenTheResponseDoesNotHaveAnError()
		{
			Assert.That(_apiResponse.HasError, Is.False);
		}

		[Test]
		public void ThenTheAppConfigHasBeenUpdated()
		{
			Assert.Multiple(() =>
			{
				Assert.That(_updatedAppConfig.ApplicationName, Is.EqualTo(_updateAppConfigRequest.AppName));
				Assert.That(_updatedAppConfig.MemorableDate, Is.EqualTo(_updateAppConfigRequest.Date));
				Assert.That(_updatedAppConfig.ImageText, Is.EqualTo(_updateAppConfigRequest.ImageText));
				Assert.That(_updatedAppConfig.LoginImageUrl, Is.EqualTo(_updateAppConfigRequest.ImageUrl));
			});
		}
	}
}
