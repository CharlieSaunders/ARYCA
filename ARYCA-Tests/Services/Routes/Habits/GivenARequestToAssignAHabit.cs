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
	public sealed class GivenARequestToAssignAHabit
	{
		private IServicesResponse _apiResponse;
		private HabitsController _habitsController;
		private DataContext _datacontext;

		private User _user;
		private Habit _habit;

		private UserHabits _assignedHabitSubject;

		[OneTimeSetUp]
		public void Setup()
		{
			_user = UserHelper.GetActiveUser();
			_habit = HabitHelper.GetCompletableHabit();

			using (_datacontext = DatabaseHelper.GetDatabase())
			{
				_datacontext = DatabaseHelper.SeedUsers(_datacontext, _user);
				_datacontext = DatabaseHelper.SeedHabits(_datacontext, _habit);

				_habitsController = ControllerHelper.GetHabitController(_datacontext,
					HttpContextHelper.GetWithArycaUserReference(_user.UserReference.ToString())
				);

				var subject = _habitsController.AssignHabit(_habit.Reference).Result as ObjectResult;
				_apiResponse = (IServicesResponse)subject.Value;

				_assignedHabitSubject = _datacontext.UserHabits.FirstOrDefault(x => x.UserReference == _user.UserReference);
			}
		}

		[Test]
		public void ThenTheResponseDoesNotHaveAnError()
		{
			Assert.That(_apiResponse.HasError, Is.False);
		}

		[Test]
		public void ThenAUserHabitHasBeenAssigned()
		{
			Assert.Multiple(() =>
			{
				Assert.That(_assignedHabitSubject.HabitReference, Is.EqualTo(_habit.Reference));
				Assert.That(_assignedHabitSubject.CanBeCompleted(), Is.EqualTo(_habit.CanBeCompleted()));
				Assert.That(_assignedHabitSubject.LastCompleted, !Is.EqualTo(_habit.LastCompleted));
			});
		}
	}
}
