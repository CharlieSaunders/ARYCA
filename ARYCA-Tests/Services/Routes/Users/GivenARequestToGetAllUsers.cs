using ARYCA_Tests.Helpers;
using Common.Classes;
using Common.Data;
using Common.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Services.Controllers;
using Services.Helpers;

namespace ARYCA_Tests.Services.Routes.Users
{
	[TestFixture]
	public sealed class GivenARequestToGetAllUsers
	{
		private IServicesResponse _apiResponse;
		private UsersController _usersController;
		private DataContext _dataContext;

		private User _user1;

		[OneTimeSetUp]
		public void Setup()
		{
			_user1 = UserHelper.GetActiveUser();

			using (_dataContext = DatabaseHelper.GetDatabase())
			{
				_dataContext = DatabaseHelper.SeedUsers(_dataContext, new List<User>() { _user1 });

				var context = HttpContextHelper.GetWithArycaUserReference(_user1.UserReference.ToString());
				_usersController = new UsersController(_dataContext, new Mock<ITokenService>().Object)
				{
					ControllerContext = new ControllerContext()
					{
						HttpContext = context
					},
				};

				var subject = _usersController.GetAll().Result as ObjectResult;
				_apiResponse = (IServicesResponse)subject.Value;
			}
		}

		[Test]
		public void TheTheResponseDoesNotHaveAnError()
		{
			Assert.That(_apiResponse.HasError, Is.False);
		}
	}
}
