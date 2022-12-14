using Client.States.Toast;
using Client.States.Toast.Types;
using Common.DTO.Configs;
using Common.Entities.Configuration;
using Common.HttpClients;
using Newtonsoft.Json;

namespace Client.ServicesBridge
{
	public class ConfigManagerBridge
	{
		private readonly IGenericHttpClient _genericHttpClient;
		private readonly ToasterService _toasterService;

		private readonly string _configUrl = "/Public/Config";

		public ConfigManagerBridge(IGenericHttpClient genericHttpClient, ToasterService toasterService)
		{
			_toasterService = toasterService;
			_genericHttpClient = genericHttpClient;
		}

		public async Task<ApplicationConfiguration> GetAppConfig(string jwToken)
		{
			var appConfig = new ApplicationConfiguration();
			try
			{
				var apiResponse = await _genericHttpClient.GetAsyncConvertResult($"{_configUrl}/Application", jwToken);

				if (apiResponse is not null)
					appConfig = JsonConvert.DeserializeAnonymousType<ApplicationConfiguration>(apiResponse.Results.ToString(), appConfig);
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Get App Config", $"Failed to get app config", MessageColour.Danger, 5));
			}

			return appConfig;
		}

		public async Task UpdateAppConfig(UpdateAppConfigRequest request, string jwToken)
		{
			var added = await _genericHttpClient.PutAsync($"{_configUrl}/Application", request, jwToken);

			if (added)
				_toasterService.AddToast(SimpleToast.NewToast("Update App Config", $"Successfully updated app config ", MessageColour.Success, 5));
			else
				_toasterService.AddToast(SimpleToast.NewToast("Update App Config", $"Failed to update app config", MessageColour.Danger, 5));
		}
	}
}
