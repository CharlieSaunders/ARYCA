using Client.States.Toast;
using Client.States.Toast.Types;
using Common.Classes.Users;
using Common.DTO.Users;
using Common.Entities.Users;
using Common.HttpClients;
using Newtonsoft.Json;

namespace Client.ServicesBridge
{
	public class UsersBridge
	{
		private readonly GenericHttpClient _genericHttpClient;
		private readonly ToasterService _toasterService;

		private readonly string _usersApiUrl = "/Public/Users";

		public UsersBridge(GenericHttpClient httpClient, ToasterService toasterService)
		{
			_genericHttpClient = httpClient;
			_toasterService = toasterService;
		}

		public async Task<User> CreateSession(string username)
		{
			var user = new User();
			try
			{
				var apiResponse = await _genericHttpClient.PostAsyncConvertResult($"{_usersApiUrl}/Session/{username}", "{ }");

				if (apiResponse is not null && !apiResponse.HasError)
					user = JsonConvert.DeserializeObject<User>(apiResponse.Results.ToString());
				else
					_toasterService.AddToast(SimpleToast.NewToast("Login", $"Failed to login with the details given.", MessageColour.Danger, 5));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				_toasterService.AddToast(SimpleToast.NewToast("Create Session", $"Unknown error getting session for user", MessageColour.Danger, 5));
			}

			return user;
		}

		public async Task<UserResponse> GetUserByReference(Guid reference, string jwToken)
		{
			var user = new UserResponse();

			try
			{
				var apiResponse = await _genericHttpClient.GetAsyncConvertResult($"{_usersApiUrl}/{reference}", jwToken);

				if (apiResponse is not null)
					user = JsonConvert.DeserializeObject<UserResponse>(apiResponse.Results.ToString());
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Get User", $"Unknown error getting user", MessageColour.Danger, 5));
			}

			return user;
		}

		public async Task<List<SlimUser>> GetSlimUsers(string jwToken)
		{
			var users = new List<SlimUser>();

			try
			{
				var apiResponse = await Task.FromResult(_genericHttpClient.GetAsyncConvertResult($"{_usersApiUrl}/all", jwToken)).Result;

				if (apiResponse is not null)
					users = JsonConvert.DeserializeObject<List<SlimUser>>(apiResponse.Results.ToString());
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Get Users", $"Unknown error getting users", MessageColour.Danger, 5));
			}

			return users;
		}

		public async Task Update(UpdateUserRequest request, string jwToken)
		{
			var updated = false;
			try
			{
				updated = await Task.FromResult(_genericHttpClient.PutAsync($"{_usersApiUrl}", request, jwToken)).Result;
			}
			catch
			{
				_toasterService.AddToast(SimpleToast.NewToast("Update User", $"Unknown error updating user", MessageColour.Danger, 5));
			}

			if (updated)
				_toasterService.AddToast(SimpleToast.NewToast("Update User", $"Successfully updated {request.FirstName}", MessageColour.Success, 5));
			else
				_toasterService.AddToast(SimpleToast.NewToast("Update User", $"Failed to update {request.FirstName}", MessageColour.Danger, 5));
		}
	}
}
