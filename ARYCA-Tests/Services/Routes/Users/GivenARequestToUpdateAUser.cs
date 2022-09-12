using ARYCA_Tests.Helpers;
using ARYCA_Tests.Helpers.TypeHelpers;
using Common.Classes;
using Common.Data;
using Common.DTO.Users;
using Common.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Services.Controllers;
using Services.Helpers;

namespace ARYCA_Tests.Services.Routes.Users
{
	[TestFixture]
	public sealed class GivenARequestToUpdateAUser
	{
		private IServicesResponse _apiResponse;
		private UsersController _usersController;
		private User _user;
		private User _updatedUser;
		private UpdateUserRequest _updateUserRequest;
		private DataContext _dataContext;

		[OneTimeSetUp]
		public void Setup()
		{
			_user = UserHelper.GetActiveUser();

			_updateUserRequest = new UpdateUserRequest
			{
				Username = "NewUsername",
				FirstName = "NewFirstName",
				SecondName = "NewSecondName",
				AvatarUrl = "NewUrl",
				Theme = "NewTheme",
				Title = "NewTitle",
				ParticleEffect = "NewParticleEffect",
				FontFamily = "NewFontFamily"
			};

			using (_dataContext = DatabaseHelper.GetDatabase())
			{
				_dataContext = DatabaseHelper.SeedUsers(_dataContext, new List<User>() { _user });

				var context = HttpContextHelper.GetWithArycaUserReference(_user.UserReference.ToString());
				_usersController = new UsersController(_dataContext, new Mock<ITokenService>().Object)
				{
					ControllerContext = new ControllerContext()
					{
						HttpContext = context
					},
				};

				var subject = _usersController.UpdateUser(_updateUserRequest).Result as ObjectResult;

				_apiResponse = (IServicesResponse)subject.Value;
				_updatedUser = (User)_apiResponse.Results;
			}

		}

		[Test]
		public void ThenTheResposneDoesNotHaveAnError()
		{
			Assert.That(_apiResponse.HasError, Is.False);
		}

		[Test]
		public void ThenTheUserReferenceHasNotBeenUdpated()
		{
			Assert.That(_updatedUser.UserReference, Is.EqualTo(_user.UserReference));
		}

		[Test]
		public void ThenTheUsernameHasBeenUdpated()
		{
			Assert.That(_updatedUser.Username, Is.EqualTo(_updateUserRequest.Username));
		}

		[Test]
		public void ThenTheFirstNameHasBeenUdpated()
		{
			Assert.That(_updatedUser.FirstName, Is.EqualTo(_updateUserRequest.FirstName));
		}

		[Test]
		public void ThenTheSecondNameHasBeenUdpated()
		{
			Assert.That(_updatedUser.SecondName, Is.EqualTo(_updateUserRequest.SecondName));
		}

		[Test]
		public void ThenTheAvatarHasBeenUdpated()
		{
			Assert.That(_updatedUser.AvatarUrl, Is.EqualTo(_updateUserRequest.AvatarUrl));
		}

		[Test]
		public void ThenTheThemeHasBeenUdpated()
		{
			Assert.That(_updatedUser.Theme, Is.EqualTo(_updateUserRequest.Theme));
		}

		[Test]
		public void ThenTheTitleHasBeenUdpated()
		{
			Assert.That(_updatedUser.Title, Is.EqualTo(_updateUserRequest.Title));
		}

		[Test]
		public void ThenTheParticleEffectHasBeenUpdated()
		{
			Assert.That(_updatedUser.ParticleEffect, Is.EqualTo(_updateUserRequest.ParticleEffect));
		}

		[Test]
		public void ThenTheFontFamilyHasBeenUpdated()
		{
			Assert.That(_updatedUser.FontFamily, Is.EqualTo(_updateUserRequest.FontFamily));
		}
	}
}
