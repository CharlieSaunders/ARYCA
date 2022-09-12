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
	public sealed class GivenARequestToUpdateAHabit
	{
		private IServicesResponse _apiResponse;
		private HabitsController _habitsController;
		private DataContext _datacontext;

		private User _user;
		private Habit _habit;
		private Habit _apiResponseHabitSubject;
		private UpdateHabitRequest _request;

		private int HABIT_VALUE = 1;

		[OneTimeSetUp]
		public void Setup()
		{
			_user = UserHelper.GetActiveUser();
			_habit = HabitHelper.GetCompletableHabit();

			_request = new UpdateHabitRequest()
			{
				HabitReference = _habit.Reference,
				Name = "New Habit name",
				Description = "New Habit description"
			};

			using (_datacontext = DatabaseHelper.GetDatabase())
			{
				_datacontext = DatabaseHelper.SeedUsers(_datacontext, _user);
				_datacontext = DatabaseHelper.SeedHabits(_datacontext, _habit);

				_habitsController = ControllerHelper.GetHabitController(_datacontext,
					HttpContextHelper.GetWithArycaUserReference(_user.UserReference.ToString())
				);

				var subject = _habitsController.UpdateHabit(_request).Result as ObjectResult;
				_apiResponse = (IServicesResponse)subject.Value;

				_apiResponseHabitSubject = _datacontext.Habits.FirstOrDefault(x => x.Reference == _habit.Reference);
			}
		}

		[Test]
		public void ThenTheResponseDoesNotHaveAnError()
		{
			Assert.That(_apiResponse.HasError, Is.False);
		}

		[Test]
		public void ThenTheResponseReturnsTheUpdatedHabit()
		{
			var apiResponseHabit = (Habit)_apiResponse.Results;
			Assert.Multiple(() =>
			{
				Assert.That(apiResponseHabit.Reference, Is.EqualTo(_habit.Reference));
				Assert.That(apiResponseHabit.Name, Is.EqualTo(_request.Name));
				Assert.That(apiResponseHabit.Description, Is.EqualTo(_request.Description));
				Assert.That(apiResponseHabit.Value, Is.EqualTo(HABIT_VALUE));
			});
		}

		[Test]
		public void ThenTheHabitHasBeenUpdated()
		{
			Assert.Multiple(() =>
			{
				Assert.That(_apiResponseHabitSubject.Name, Is.EqualTo(_request.Name));
				Assert.That(_apiResponseHabitSubject.Description, Is.EqualTo(_request.Description));
				Assert.That(_apiResponseHabitSubject.Value, Is.EqualTo(HABIT_VALUE));
			});
		}
	}
}
