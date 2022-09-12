using Client.States.Toast;
using Client.States.Toast.Types;
using Common.DTO.Achievements;
using Common.HttpClients;
using Newtonsoft.Json;

namespace Client.ServicesBridge
{
	public class AchievementsBridge
	{
		private readonly IGenericHttpClient _genericHttpClient;
		private readonly ToasterService _toasterService;

		private readonly string _achievementUrl = "/Public/Achievements";

		public AchievementsBridge(IGenericHttpClient genericHttpClient, ToasterService toasterService)
		{
			_toasterService = toasterService;
			_genericHttpClient = genericHttpClient;
		}

		public async Task<List<UserAchievementResponse>> GetUserAchievements(string jwToken)
		{
			var userAchievements = new List<UserAchievementResponse>();
			try
			{
				var apiResponse = await _genericHttpClient.GetAsyncConvertResult($"{_achievementUrl}/User", jwToken);
				if (apiResponse is not null && !apiResponse.HasError)
					userAchievements = JsonConvert.DeserializeAnonymousType<List<UserAchievementResponse>>(apiResponse.Results.ToString(), userAchievements);
				else
					_toasterService.AddToast(SimpleToast.NewToast("Get Achievements", $"Failed to get all user achievements.", MessageColour.Danger, 5));
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Get Achievements", $"Unknown error getting all user achievements.", MessageColour.Danger, 5));
			}

			return userAchievements;
		}

		public async Task UnlockAchievement(string jwToken, int achievementId)
		{
			try
			{
				await _genericHttpClient.PutAsync($"{_achievementUrl}/User/{achievementId}", "{}", jwToken);
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Unlock Achievement", $"Unknown error unlocking achievement", MessageColour.Danger, 5));
			}
		}
	}
}
