using ARYCA_Tests.Helpers;
using ARYCA_Tests.Helpers.TypeHelpers;
using Common.Classes;
using Common.Classes.Achievements;
using Common.Data;
using Common.Entities.Achievements;
using Common.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Services.Controllers;

namespace ARYCA_Tests.Services.Routes.Achievements
{
	[TestFixture]
	public sealed class GivenARequestToGetAllAchievements
	{
		private IServicesResponse _apiResponse;
		private List<Achievement> _apiResponseAchievements;
		private AchievementsController _achievementController;
		private DataContext _datacontext;

		private User _user;
		private List<Achievement> _achievements;

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

				var subject = _achievementController.GetAll().Result as ObjectResult;
				_apiResponse = (IServicesResponse)subject.Value;
				_apiResponseAchievements = (List<Achievement>)_apiResponse.Results;
			}
		}

		[Test]
		public void ThenTheResponseDoesNotHaveAnError()
		{
			Assert.That(_apiResponse.HasError, Is.False);
		}

		[Test]
		public void ThenTheAchievementsMatchTheHardcodedAchievements()
		{
			var index = 0;
			foreach (var achievement in _achievements)
			{
				Assert.Multiple(() =>
				{
					Assert.That(_apiResponseAchievements[index].Identifier, Is.EqualTo(achievement.Identifier));
					Assert.That(_apiResponseAchievements[index].Title, Is.EqualTo(achievement.Title));
					Assert.That(_apiResponseAchievements[index].Description, Is.EqualTo(achievement.Description));
					Assert.That(_apiResponseAchievements[index].ImageUrl, Is.EqualTo(achievement.ImageUrl));
				});
				index += 1;
			}
		}
	}
}
