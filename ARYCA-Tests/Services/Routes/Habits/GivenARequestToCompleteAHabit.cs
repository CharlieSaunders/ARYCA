using ARYCA_Tests.Helpers;
using ARYCA_Tests.Helpers.TypeHelpers;
using Common.Classes;
using Common.Data;
using Common.Entities.Habits;
using Common.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Services.Controllers;


namespace ARYCA_Tests.Services.Routes.Habits
{
	[TestFixture]
	public sealed class GivenARequestToCompleteAHabit
	{
		private IServicesResponse _apiResponse;
		private HabitsController _habitsController;
		private DataContext _datacontext;

		private User _user;
		private User _userSubject;
		private Habit _habit;
		private UserHabits _userHabits;
		private UserHabits _updatedHabitSubject;

		[OneTimeSetUp]
		public void Setup()
		{
			_user = UserHelper.GetActiveUser();
			_habit = HabitHelper.GetCompletableHabit();
			_userHabits = HabitHelper.GetCompletableUserHabit(_user.UserReference, _habit.Reference);

			using (_datacontext = DatabaseHelper.GetDatabase())
			{
				_datacontext = DatabaseHelper.SeedUsers(_datacontext, _user);
				_datacontext = DatabaseHelper.SeedHabits(_datacontext, _habit);
				_datacontext = DatabaseHelper.SeedUserHabits(_datacontext, _userHabits);

				_habitsController = ControllerHelper.GetHabitController(_datacontext,
					HttpContextHelper.GetWithArycaUserReference(_user.UserReference.ToString())
				);

				var subject = _habitsController.CompleteHabit(_habit.Reference).Result as ObjectResult;
				_apiResponse = (IServicesResponse)subject.Value;

				_updatedHabitSubject = _datacontext.UserHabits.FirstOrDefault(x => x.HabitReference == _habit.Reference && x.UserReference == _user.UserReference);
				_userSubject = _datacontext.Users.FirstOrDefault(x => x.UserReference == _user.UserReference);
			}
		}

		[Test]
		public void ThenTheResponseDoesNotHaveAnError()
		{
			Assert.That(_apiResponse.HasError, Is.False);
		}

		[Test]
		public void ThenTheHabitIsNoLongerCompletable()
		{
			Assert.Multiple(() =>
			{
				Assert.That(_updatedHabitSubject.CanBeCompleted(), Is.False);
				Assert.That(_updatedHabitSubject.LastCompleted, Is.GreaterThan(DateTime.Now.AddHours(-1)));
			});
		}

		[Test]
		public void ThenTheUserBalanceHasBeenUpdated()
		{
			Assert.That(_userSubject.Balance, Is.EqualTo(101));
		}
	}
}
