using Client.States.Toast;
using Client.States.Toast.Types;
using Common.DTO.Achievements;
using Common.Entities.Achievements;
using Common.HttpClients;
using Newtonsoft.Json;

namespace Client.ServicesBridge
{
	public class AchievementsBridge
	{
		private readonly GenericHttpClient _genericHttpClient;
		private readonly ToasterService _toasterService;

		private readonly string _achievementUrl = "/Public/Achievements";

		public AchievementsBridge(GenericHttpClient genericHttpClient, ToasterService toasterService)
		{
			_toasterService = toasterService;
			_genericHttpClient = genericHttpClient;
		}

		public async Task<List<UserAchievementResponse>> GetUserAchievements(string jwToken)
		{
			var userAchievements = new List<UserAchievementResponse>();

			try
			{
				var apiResponse = await Task.FromResult(_genericHttpClient.GetAsyncConvertResult($"{_achievementUrl}/User", jwToken)).Result;

				if (apiResponse is not null && !apiResponse.HasError)
					userAchievements = JsonConvert.DeserializeObject<List<UserAchievementResponse>>(apiResponse.Results.ToString());
				else
					_toasterService.AddToast(SimpleToast.NewToast("Get Achievements", $"Failed to get all user achievements.", MessageColour.Danger, 5));
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Get Achievements", $"Unknown error getting all user achievements.", MessageColour.Danger, 5));
			}

			return userAchievements;
		}

		public async Task<List<Achievement>> GetAchievements(string jwToken)
		{
			var achievements = new List<Achievement>();

			try
			{
				var apiResponse = await Task.FromResult(_genericHttpClient.GetAsyncConvertResult($"{_achievementUrl}", jwToken)).Result;

				if (apiResponse is not null && !apiResponse.HasError)
					achievements = JsonConvert.DeserializeObject<List<Achievement>>(apiResponse.Results.ToString());
				else
					_toasterService.AddToast(SimpleToast.NewToast("Get Achievements", $"Failed to get all achievements.", MessageColour.Danger, 5));
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Get Achievements", $"Unknown error getting all achievements.", MessageColour.Danger, 5));
			}

			return achievements;
		}

		public async Task UnlockAchievement(string jwToken, int achievementId)
		{
			try
			{
				await Task.FromResult(_genericHttpClient.PutAsync($"{_achievementUrl}/User/{achievementId}", "{}", jwToken)).Result;
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Unlock Achievement", $"Unknown error unlocking achievement", MessageColour.Danger, 5));
			}
		}
	}
}
