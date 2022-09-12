using ARYCA_Tests.Helpers;
using ARYCA_Tests.Helpers.TypeHelpers;
using Common.Classes;
using Common.Classes.ErrorHandling;
using Common.Data;
using Common.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Services.Controllers;
using Services.Helpers;

namespace ARYCA_Tests.Services.Authorization
{
	[TestFixture]
	public sealed class GivenARequestWithMismatchingReferenceAndJwt
	{
		private UsersController _usersController;
		private DataContext _dataContext;
		private User _user1;

		[Test]
		public void Setup()
		{
			_user1 = UserHelper.GetInactiveUser("Test", 4);

			using (_dataContext = DatabaseHelper.GetDatabase())
			{
				_dataContext = DatabaseHelper.SeedUsers(_dataContext, _user1);

				var context = HttpContextHelper.GetWithArycaUserReference(_user1.UserReference.ToString());
				_usersController = new UsersController(_dataContext, new Mock<ITokenService>().Object)
				{
					ControllerContext = new ControllerContext()
					{
						HttpContext = context
					},
				};

				var subject = _usersController.CreateAuthenticationToken(_user1.Username).Result as ObjectResult;
				var apiResponse = (IServicesResponse)subject.Value;
				var user = (User)apiResponse.Results;

				var newContext = HttpContextHelper.GetWithArycaUserReferenceAndJwt(Guid.Empty.ToString(), user.AuthenticationToken);
				_usersController = new UsersController(_dataContext, new Mock<ITokenService>().Object)
				{
					ControllerContext = new ControllerContext()
					{
						HttpContext = newContext
					},
				};

				Assert.Throws<Exception>(() => _usersController.GetAll(), Error.Users.NoActiveSession);
			}
		}
	}
}
