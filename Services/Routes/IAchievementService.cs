using Common.Classes;
using Common.Classes.Achievements;
using Common.Data;
using Common.DTO.Achievements;
using Common.DTO.Logs;
using Common.Entities.Achievements;
using Common.Entities.Logs;
using Infrastructure.EF;
using Infrastructure.EF.Achievements;
using Infrastructure.EF.Unlockables;
using Infrastructure.Interfaces;
using static Common.Classes.Achievements.AchievementHelper;

namespace Services.Routes
{
	public interface IAchievementService
	{
		public IServicesResponse GetAll();
		public IServicesResponse GetForUser(Guid userReference);
		public IServicesResponse Unlock(Guid userReference, int achievementId);
	}

	public class AchievementService : IAchievementService
	{
		private readonly IAchievementRepository _achievementRepository;
		private readonly EFUsersRepositories _userRepository;
		private readonly IUnlockablesRepository _unlockableRepository;
		private readonly ILogsService _logService;

		public AchievementService(DataContext db)
		{
			_achievementRepository = new EFAchievementRepository(db);
			_userRepository = new EFUsersRepositories(db);
			_unlockableRepository = new EFUnlockablesRepository(db);
			_logService = new LogsService(db);
		}

		public IServicesResponse GetForUser(Guid userReference)
		{
			var response = new IServicesResponse(new List<UserAchievementResponse>());
			try
			{
				var allUserAchievements = _userRepository.GetAchievementsForUser(userReference);
				var allAchievements = AchievementHelper.GetAll();

				foreach (var achievement in allUserAchievements)
				{
					var matching = allAchievements.First(x => x.Identifier == achievement.AchievementReference);
					response.Results.Add(new UserAchievementResponse(achievement, matching));
				}
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to get all user achievements", this.GetType()));
			}

			return response;
		}

		public IServicesResponse GetAll()
		{
			var response = new IServicesResponse(new List<Achievement>());
			try
			{
				var all = _achievementRepository.GetAll();
				var allAchievements = AchievementHelper.GetAll();
				if (all.Count != allAchievements.Count)
				{
					response.Results = allAchievements;

					foreach (var achievement in allAchievements)
					{
						var dbAchievement = _achievementRepository.GetById(achievement.Identifier);
						if (!dbAchievement.IsValid())
							_achievementRepository.Add(achievement.Title, achievement.Identifier, achievement.Description, achievement.ImageUrl);
					}
				}
				else
					response.Results = all;
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to get all achievements", this.GetType()));
			}

			return response;
		}

		public IServicesResponse Unlock(Guid userReference, int achievementId)
		{
			var response = new IServicesResponse(false);

			var all = _achievementRepository.GetAll();
			if (all.Count == 0)
				GetAll();

			try
			{
				var achievement = _achievementRepository.GetById(achievementId);
				if (achievement.IsValid())
				{
					_userRepository.AddAchievement(userReference, achievementId);

					ItemUnlocksForAchievements(userReference, achievement);
					response.Results = true;
				}
				else
				{
					response.AddError("No Achievement found", "Unable to find a matching achievement");
					_logService.Add(new CreateLogRequest(this.GetType().ToString(), $"Unable to find achievement by id {achievementId}"));
				}
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to get add usser achievements", this.GetType()));
			}

			return response;
		}

		private void ItemUnlocksForAchievements(Guid userReference, Achievement achievement)
		{
			if (achievement.Title == AchievementType.TradeProfit100)
			{
				// unlock poly wolf
				var unlockable = _unlockableRepository.GetByName("Poly Wolf");
				var userUnlock = _userRepository.GetUnlockables(userReference, unlockable.Reference);
				if (userUnlock.IsEmpty())
					_userRepository.AddUnlockable(userReference, unlockable.Reference);
			}

			if (achievement.Title == AchievementType.TradeProfit1000)
			{
				// unlock trade profit particle
				var unlockable = _unlockableRepository.GetByName("💵 🤑 💲");
				var userUnlock = _userRepository.GetUnlockables(userReference, unlockable.Reference);
				if (userUnlock.IsEmpty())
					_userRepository.AddUnlockable(userReference, unlockable.Reference);
			}
		}
	}
}
