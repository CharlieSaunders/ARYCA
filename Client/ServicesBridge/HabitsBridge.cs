using Client.States.Toast;
using Client.States.Toast.Types;
using Common.DTO.Habits;
using Common.Entities.Habits;
using Common.HttpClients;
using Newtonsoft.Json;

namespace Client.ServicesBridge
{
	public class HabitsBridge
	{
		private readonly GenericHttpClient _genericHttpClient;
		private readonly ToasterService _toasterService;

		private readonly string _habitsApiUrl = "/Public/Habits";

		public HabitsBridge(GenericHttpClient genericHttpClient, ToasterService toasterService)
		{
			_toasterService = toasterService;
			_genericHttpClient = genericHttpClient;
		}

		public async Task<List<Habit>> GetAllHabits(string jwToken)
		{
			var habits = new List<Habit>();

			try
			{
				var apiResponse = await Task.FromResult(_genericHttpClient.GetAsyncConvertResult(_habitsApiUrl, jwToken)).Result;

				if (apiResponse is not null)
					habits = JsonConvert.DeserializeObject<List<Habit>>(apiResponse.Results.ToString());
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Get All Habits", $"Failed to get all habits", MessageColour.Danger, 5));
			}


			return habits;
		}

		public async Task<List<Habit>> GetAllHabitsAssigned(string jwToken)
		{
			var habits = new List<Habit>();

			try
			{
				var apiResponse = await Task.FromResult(_genericHttpClient.GetAsyncConvertResult($"{_habitsApiUrl}/Assigned", jwToken)).Result;

				if (apiResponse is not null)
					habits = JsonConvert.DeserializeObject<List<Habit>>(apiResponse.Results.ToString());
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Get Assigned Habits", $"Failed to get assigned habits", MessageColour.Danger, 5));
			}

			return habits;
		}

		public async Task AddHabit(CreateHabitRequest request, string habitName, string jwToken)
		{
			var added = false;
			try
			{
				added = await Task.FromResult(_genericHttpClient.PostAsync(_habitsApiUrl, request, jwToken)).Result;
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Add Habit", $"Unknown error adding habit", MessageColour.Danger, 5));
			}

			if (added)
				_toasterService.AddToast(SimpleToast.NewToast("Add Habit", $"Successfully added {habitName} habit", MessageColour.Success, 5));
			else
				_toasterService.AddToast(SimpleToast.NewToast("Add Habit", $"Failed to added {habitName} habit", MessageColour.Danger, 5));
		}

		public async Task DeleteHabit(Guid reference, string jwToken)
		{
			var deleted = false;
			try
			{
				deleted = await Task.FromResult(_genericHttpClient.DeleteAsyncInUrl($"{_habitsApiUrl}/{reference}", jwToken)).Result;
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Delete Habit", $"Unknown error deleting habit", MessageColour.Danger, 5));
			}

			if (deleted)
				_toasterService.AddToast(SimpleToast.NewToast("Delete Habit", $"Successfully deleted habit", MessageColour.Success, 5));
			else
				_toasterService.AddToast(SimpleToast.NewToast("Delete Habit", $"Failed to deleted habit", MessageColour.Danger, 5));
		}

		public async Task Unassign(Guid reference, string habitName, string jwToken)
		{
			var unassigned = false;
			try
			{
				unassigned = await Task.FromResult(_genericHttpClient.PutAsync($"{_habitsApiUrl}/Unassign/{reference}", "{}", jwToken)).Result;
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Unassign Habit", $"Unknown error unassigning habit", MessageColour.Danger, 5));
			}

			if (unassigned)
				_toasterService.AddToast(SimpleToast.NewToast("Unassign Habit", $"Successfully unassigned {habitName} habit", MessageColour.Success, 5));
			else
				_toasterService.AddToast(SimpleToast.NewToast("Unassign Habit", $"Failed to unassign {habitName} habit", MessageColour.Danger, 5));
		}

		public async Task Assign(Guid reference, string habitName, string jwToken)
		{
			var assigned = false;
			try
			{
				assigned = await Task.FromResult(_genericHttpClient.PutAsync($"{_habitsApiUrl}/Assign/{reference}", "{}", jwToken)).Result;
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Assign Habit", $"Unknown error assigning habit", MessageColour.Danger, 5));
			}

			if (assigned)
				_toasterService.AddToast(SimpleToast.NewToast("Assign Habit", $"Successfully assigned {habitName} habit", MessageColour.Success, 5));
			else
				_toasterService.AddToast(SimpleToast.NewToast("Assign Habit", $"Failed to assign {habitName} habit", MessageColour.Danger, 5));
		}

		public async Task Complete(Guid reference, string habitName, string jwToken)
		{
			var completed = false;
			try
			{
				completed = await Task.FromResult(_genericHttpClient.PostAsync($"{_habitsApiUrl}/Complete/{reference}", "{}", jwToken)).Result;
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Complete Habit", $"Unknown error completing habit", MessageColour.Danger, 5));
			}

			if (completed)
				_toasterService.AddToast(SimpleToast.NewToast("Complete Habit", $"Successfully completed {habitName} habit", MessageColour.Success, 5));
			else
				_toasterService.AddToast(SimpleToast.NewToast("Complete Habit", $"Failed to completed {habitName} habit", MessageColour.Danger, 5));
		}

		public async Task Update(UpdateHabitRequest request, string habitName, string jwToken)
		{
			var updated = false;
			try
			{
				updated = await Task.FromResult(_genericHttpClient.PutAsync($"{_habitsApiUrl}", request, jwToken)).Result;
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Update Habit", $"Unknown error updating habit", MessageColour.Danger, 5));
			}

			if (updated)
				_toasterService.AddToast(SimpleToast.NewToast("Update Habit", $"Successfully updated {habitName} habit", MessageColour.Success, 5));
			else
				_toasterService.AddToast(SimpleToast.NewToast("Update Habit", $"Failed to update {habitName} habit", MessageColour.Danger, 5));
		}
	}
}
