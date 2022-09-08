using ARYCA_Tests.Helpers;
using Common.Classes;
using Common.Classes.Achievements;
using Common.Data;
using Common.DTO.Achievements;
using Common.Entities.Achievements;
using Common.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Services.Controllers;

namespace ARYCA_Tests.Services.Routes.Achievements
{
	[TestFixture]
	public sealed class GivenARequestToUnlockAnAchievement
	{
		private IServicesResponse _apiResponse;
		private AchievementsController _achievementController;
		private DataContext _datacontext;

		private User _user;
		private List<Achievement> _achievements;
		private List<UserAchievementResponse> _userAchievements;

		[OneTimeSetUp]
		public void Setup()
		{
			_user = UserHelper.GetActiveUser();
			_achievements = AchievementHelper.GetAll();

			using (_datacontext = DatabaseHelper.GetDatabase())
			{
				_datacontext = DatabaseHelper.SeedUsers(_datacontext, _user);
				_datacontext = DatabaseHelper.SeedAchievements(_datacontext, _achievements);

				_achievementController = ControllerHelper.GetAchievementsController(_datacontext,
					HttpContextHelper.GetWithArycaUserReference(_user.UserReference.ToString())
				);

				var subject = _achievementController.UnlockAchievement(_achievements.First().Identifier).Result as ObjectResult;
				_apiResponse = (IServicesResponse)subject.Value;

				var userAchievementsSubject = _achievementController.GetAllForUser().Result as ObjectResult;
				var castedValue = (IServicesResponse)userAchievementsSubject.Value;
				_userAchievements = (List<UserAchievementResponse>)castedValue.Results;
			}
		}

		[Test]
		public void ThenTheResponseDoesNotHaveAnError()
		{
			Assert.That(_apiResponse.HasError, Is.False);
		}

		[Test]
		public void ThenTheAchievementWasUnlocked()
		{
			Assert.That(_userAchievements, Has.Count.EqualTo(1));
		}

		[Test]
		public void ThenTheCorrectAchievementWasUnlocked()
		{
			var achievement = _achievements.First();
			var apiResponse = _userAchievements.First();
			Assert.Multiple(() =>
			{
				Assert.That(apiResponse.Title, Is.EqualTo(achievement.Title));
				Assert.That(apiResponse.Description, Is.EqualTo(achievement.Description));
				Assert.That(apiResponse.ImageUrl, Is.EqualTo(achievement.ImageUrl));
				Assert.That(apiResponse.TimesUnlocked, Is.EqualTo(1));
			});
		}
	}
}
