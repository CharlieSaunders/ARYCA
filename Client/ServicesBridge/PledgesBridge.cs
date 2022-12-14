using Client.States.Toast;
using Client.States.Toast.Types;
using Common.DTO.Pledges;
using Common.Entities.Pledges;
using Common.HttpClients;
using Newtonsoft.Json;

namespace Client.ServicesBridge
{
	public class PledgesBridge
	{
		private readonly IGenericHttpClient _genericHttpClient;
		private readonly ToasterService _toasterService;
		private readonly string _habitsApiUrl = "/Public/Pledges";

		public PledgesBridge(IGenericHttpClient httpClient, ToasterService toasterService)
		{
			_genericHttpClient = httpClient;
			_toasterService = toasterService;
		}

		public async Task<List<Pledge>> GetAll(string jwToken)
		{
			var pledges = new List<Pledge>();

			try
			{
				var apiResponse = await _genericHttpClient.GetAsyncConvertResult(_habitsApiUrl, jwToken);

				if (apiResponse is not null)
					pledges = JsonConvert.DeserializeAnonymousType<List<Pledge>>(apiResponse.Results.ToString(), pledges);
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Get Pledges", $"Unknown error getting all pledges", MessageColour.Danger, 5));
			}

			return pledges;
		}

		public async Task<List<AssignedPledgeResponse>> GetAssigned(string jwToken)
		{
			var pledges = new List<AssignedPledgeResponse>();

			try
			{
				var apiResponse = await _genericHttpClient.GetAsyncConvertResult($"{_habitsApiUrl}/Assigned", jwToken);

				if (apiResponse is not null)
					pledges = JsonConvert.DeserializeAnonymousType<List<AssignedPledgeResponse>>(apiResponse.Results.ToString(), pledges);
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Get Assigned Pledges", $"Unknown error getting assigned pledges", MessageColour.Danger, 5));
			}

			return pledges;
		}

		public async Task Create(CreatePledgeRequest request, string jwToken)
		{
			var created = false;
			try
			{
				created = await _genericHttpClient.PostAsync(_habitsApiUrl, request, jwToken);
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Create Pledges", $"Unknown error creating pledge", MessageColour.Danger, 5));
			}

			if (created)
				_toasterService.AddToast(SimpleToast.NewToast("Create Pledge", $"Successfully created {request.Name} pledge", MessageColour.Success, 5));
			else
				_toasterService.AddToast(SimpleToast.NewToast("Create Pledge", $"Failed to create {request.Name} pledge", MessageColour.Danger, 5));
		}

		public async Task Update(UpdatePledgeRequest request, string jwToken)
		{
			var updated = false;
			try
			{
				updated = await _genericHttpClient.PutAsync(_habitsApiUrl, request, jwToken);
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Update Pledge", $"Unknown error updating pledge", MessageColour.Danger, 5));
			}


			if (updated)
				_toasterService.AddToast(SimpleToast.NewToast("Update Pledge", $"Successfully created {request.Name} pledge", MessageColour.Success, 5));
			else
				_toasterService.AddToast(SimpleToast.NewToast("Update Pledge", $"Failed to create {request.Name} pledge", MessageColour.Danger, 5));
		}

		public async Task Assign(AssignPledgeRequest request, string jwToken)
		{
			var assigned = false;

			try
			{
				assigned = await _genericHttpClient.PutAsync($"{_habitsApiUrl}/Assign", request, jwToken);
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Assign Pledge", $"Unknown error assigning pledge", MessageColour.Danger, 5));
			}

			if (assigned)
				_toasterService.AddToast(SimpleToast.NewToast("Update Pledge", $"Successfully assigned pledge", MessageColour.Success, 5));
			else
				_toasterService.AddToast(SimpleToast.NewToast("Update Pledge", $"Failed to assign pledge", MessageColour.Danger, 5));
		}

		public async Task UpdateAssignedStatus(UpdatePledgeStatusRequest request, string jwToken)
		{
			var updateStatus = false;

			try
			{
				updateStatus = await _genericHttpClient.PutAsync($"{_habitsApiUrl}/Assigned/Status", request, jwToken);
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Update Assigned Pledge", $"Unknown error updating assigned pledge", MessageColour.Danger, 5));
			}

			if (updateStatus)
				_toasterService.AddToast(SimpleToast.NewToast("Update Pledge Status", $"Successfully updated pledge status to '{request.NewStatus}'", MessageColour.Success, 5));
			else
				_toasterService.AddToast(SimpleToast.NewToast("Update Pledge Status", $"Failed to update pledge status to '{request.NewStatus}'", MessageColour.Danger, 5));
		}

		public async Task DeletePlegde(Guid pledgeReference, string jwToken)
		{
			var deleted = false;
			try
			{
				deleted = await _genericHttpClient.DeleteAsyncInUrl($"{_habitsApiUrl}/{pledgeReference}", jwToken);
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Delete Pledge", $"Unknown error deleting pledge", MessageColour.Danger, 5));
			}

			if (deleted)
				_toasterService.AddToast(SimpleToast.NewToast("Delete Status", $"Successfully deleted pledge", MessageColour.Success, 5));
			else
				_toasterService.AddToast(SimpleToast.NewToast("Delete Status", $"Failed to delete pledge", MessageColour.Danger, 5));
		}
	}
}
