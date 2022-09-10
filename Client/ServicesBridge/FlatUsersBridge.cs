using Common.Classes.Users;
using Common.HttpClients;
using Newtonsoft.Json;

namespace Client.ServicesBridge
{
	public class FlatUsersBridge
	{
		private readonly GenericHttpClient _genericHttpClient;

		private readonly string _usersApiUrl = "/Public/Users";

		public FlatUsersBridge(GenericHttpClient httpClient)
		{
			_genericHttpClient = httpClient;
		}

		public async Task<UserResponse> GetUserByReference(Guid reference, string jwToken)
		{
			var user = new UserResponse();

			if (reference == Guid.Empty)
				return user;

			var apiResponse = await Task.FromResult(_genericHttpClient.GetAsyncConvertResult($"{_usersApiUrl}/{reference}", jwToken)).Result;

			if (apiResponse is not null && !apiResponse.HasError)
				user = JsonConvert.DeserializeObject<UserResponse>(apiResponse.Results.ToString());

			return user;
		}

	}
}
