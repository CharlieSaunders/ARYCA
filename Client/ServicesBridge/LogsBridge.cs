using Client.States.Toast;
using Client.States.Toast.Types;
using Common.Entities.Logs;
using Common.HttpClients;
using Newtonsoft.Json;

namespace Client.ServicesBridge
{
	public class LogsBridge
	{
		private readonly IGenericHttpClient _genericHttpClient;
		private readonly ToasterService _toasterService;

		private readonly string _apiUrl = "/Public/Config/Logs";

		public LogsBridge(IGenericHttpClient genericHttpClient, ToasterService toasterService)
		{
			_toasterService = toasterService;
			_genericHttpClient = genericHttpClient;
		}

		public async Task<List<Log>> GetAllLogs(string jwToken)
		{
			var logs = new List<Log>();
			try
			{
				var apiResponse = await _genericHttpClient.GetAsyncConvertResult(_apiUrl, jwToken);

				if (apiResponse is not null)
					logs = JsonConvert.DeserializeAnonymousType<List<Log>>(apiResponse.Results.ToString(), logs);
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Get Logs", $"Unknown error getting all logs", MessageColour.Danger, 5));
			}


			return logs;
		}

		public async Task Delete(int logId, string jwToken)
		{
			var deleted = false;
			try
			{
				deleted = await _genericHttpClient.DeleteAsyncInUrl($"{_apiUrl}/{logId}", jwToken);
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Delete Logs", $"Unknown error deleting log", MessageColour.Danger, 5));
			}

			if (deleted)
				_toasterService.AddToast(SimpleToast.NewToast("Resolve Log", $"Successfully resolved", MessageColour.Success, 5));
			else
				_toasterService.AddToast(SimpleToast.NewToast("Resolve Log", $"Unable to resolve log", MessageColour.Danger, 5));
		}
	}
}