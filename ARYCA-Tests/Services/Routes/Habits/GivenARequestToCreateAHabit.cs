using ARYCA_Tests.Helpers;
using ARYCA_Tests.Helpers.TypeHelpers;
using Common.Classes;
using Common.Data;
using Common.DTO.Habits;
using Common.Entities.Habits;
using Common.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Services.Controllers;

namespace ARYCA_Tests.Services.Routes.Habits
{
	[TestFixture]
	public sealed class GivenARequestToCreateAHabit
	{
		private IServicesResponse _apiResponse;
		private HabitsController _habitsController;
		private DataContext _datacontext;

		private User _user;
		private CreateHabitRequest _request;
		private int HABIT_VALUE = 1;

		[OneTimeSetUp]
		public void Setup()
		{
			_user = UserHelper.GetActiveUser();
			_request = new CreateHabitRequest()
			{
				Name = "New Habit",
				Description = "New Habit description"
			};

			using (_datacontext = DatabaseHelper.GetDatabase())
			{
				_datacontext = DatabaseHelper.SeedUsers(_datacontext, _user);

				_habitsController = ControllerHelper.GetHabitController(_datacontext,
					HttpContextHelper.GetWithArycaUserReference(_user.UserReference.ToString())
				);

				var subject = _habitsController.CreateHabit(_request).Result as ObjectResult;
				_apiResponse = (IServicesResponse)subject.Value;
			}
		}

		[Test]
		public void ThenTheResponseDoesNotHaveAnError()
		{
			Assert.That(_apiResponse.HasError, Is.False);
		}

		[Test]
		public void ThenTheApiResponseContainsTheNewHabit()
		{
			var apiResponseHabit = (Habit)_apiResponse.Results;
			Assert.Multiple(() =>
			{
				Assert.That(apiResponseHabit.Name, Is.EqualTo(_request.Name));
				Assert.That(apiResponseHabit.Description, Is.EqualTo(_request.Description));
				Assert.That(apiResponseHabit.Value, Is.EqualTo(HABIT_VALUE));
			});
		}
	}
}
