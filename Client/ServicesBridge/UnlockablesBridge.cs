using Client.States.Toast;
using Client.States.Toast.Types;
using Common.DTO.Unlockables;
using Common.HttpClients;
using Newtonsoft.Json;

namespace Client.ServicesBridge
{
	public class UnlockablesBridge
	{
		private readonly GenericHttpClient _genericHttpClient;
		private readonly ToasterService _toasterService;
		private readonly string _unlockablesApiUrl = "/Public/Unlockables";

		public UnlockablesBridge(GenericHttpClient httpClient, ToasterService toasterService)
		{
			_genericHttpClient = httpClient;
			_toasterService = toasterService;
		}

		public async Task<List<UnlockableResponse>> GetAll(string jwToken)
		{
			var habits = new List<UnlockableResponse>();

			try
			{
				var apiResponse = await Task.FromResult(_genericHttpClient.GetAsyncConvertResult(_unlockablesApiUrl, false, jwToken)).Result;

				if (apiResponse is not null)
					habits = JsonConvert.DeserializeObject<List<UnlockableResponse>>(apiResponse.Results.ToString());
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Get All Unlocks", $"Unknown error getting all unlocks", MessageColour.Danger, 5));
			}

			return habits;
		}

		public async Task<List<UnlockableResponse>> GetAllUnlocked(string jwToken)
		{
			var unlocks = new List<UnlockableResponse>();

			try
			{
				var apiResponse = await Task.FromResult(_genericHttpClient.GetAsyncConvertResult($"{_unlockablesApiUrl}/Unlocked", false, jwToken)).Result;

				if (apiResponse is not null)
					unlocks = JsonConvert.DeserializeObject<List<UnlockableResponse>>(apiResponse.Results.ToString());
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Get All Unlocked", $"Unknown error getting all unlocked", MessageColour.Danger, 5));
			}

			return unlocks;
		}

		public async Task Unlock(Guid unlockReference, string itemName, string jwToken)
		{
			var unlocked = false;

			try
			{
				unlocked = await Task.FromResult(_genericHttpClient.PutAsync($"{_unlockablesApiUrl}/Unlock/{unlockReference}", "{}", false, jwToken)).Result;
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Marketplace Unlock", $"Unknown error unlocking item", MessageColour.Danger, 5));
			}

			if (unlocked)
				_toasterService.AddToast(SimpleToast.NewToast("Marketplace Unlock", $"Successfully unlocked {itemName}", MessageColour.Success, 5));
			else
				_toasterService.AddToast(SimpleToast.NewToast("Marketplace Unlock", $"Failed to unlock {itemName}", MessageColour.Danger, 5));
		}

		public async Task Gift(GiftUnlockableRequest request, string jwToken)
		{
			var unlocked = false;

			try
			{
				unlocked = await Task.FromResult(_genericHttpClient.PutAsync($"{_unlockablesApiUrl}/Unlock/Gift", request, false, jwToken)).Result;
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Marketplace Gift", $"Unknown error gifting item", MessageColour.Danger, 5));
			}

			if (unlocked)
				_toasterService.AddToast(SimpleToast.NewToast("Marketplace Gift", $"Successfully gifted item.", MessageColour.Success, 5));
			else
				_toasterService.AddToast(SimpleToast.NewToast("Marketplace Gift", $"Failed to gift item.", MessageColour.Danger, 5));
		}

		public async Task Update(UpdateUnlockableRequest request, string jwToken)
		{
			var updated = false;
			try
			{
				updated = await Task.FromResult(_genericHttpClient.PutAsync($"{_unlockablesApiUrl}/Update", request, false, jwToken)).Result;
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Marketplace Update", $"Unknown error updating item", MessageColour.Danger, 5));
			}

			if (updated)
				_toasterService.AddToast(SimpleToast.NewToast("Marketplace Update", $"Successfully updated {request.Name}.", MessageColour.Success, 5));
			else
				_toasterService.AddToast(SimpleToast.NewToast("Marketplace Update", $"Failed to update {request.Name}.", MessageColour.Danger, 5));
		}

		public async Task Delete(Guid reference, string jwToken)
		{
			var deleted = false;

			try
			{
				deleted = await Task.FromResult(_genericHttpClient.DeleteAsyncInUrl($"{_unlockablesApiUrl}/{reference}", false, jwToken)).Result;
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Marketplace Delete", $"Unknown error deleting item", MessageColour.Danger, 5));
			}

			if (deleted)
				_toasterService.AddToast(SimpleToast.NewToast("Marketplace Delete", $"Successfully deleted unlockable.", MessageColour.Success, 5));
			else
				_toasterService.AddToast(SimpleToast.NewToast("Marketplace Delete", $"Failed to delete unlockable.", MessageColour.Danger, 5));
		}
	}
}
