using Common.Classes.Users;
using Common.HttpClients;
using Newtonsoft.Json;

namespace Client.ServicesBridge
{
	public class FlatUsersBridge
	{
		private readonly IGenericHttpClient _genericHttpClient;

		private readonly string _usersApiUrl = "/Public/Users";

		public FlatUsersBridge(IGenericHttpClient httpClient)
		{
			_genericHttpClient = httpClient;
		}

		public async Task<UserResponse> GetUserByReference(Guid reference, string jwToken)
		{
			var user = new UserResponse();

			if (reference == Guid.Empty)
				return user;

			var apiResponse = await _genericHttpClient.GetAsyncConvertResult($"{_usersApiUrl}/{reference}", jwToken);

			if (apiResponse is not null && !apiResponse.HasError)
				user = JsonConvert.DeserializeAnonymousType<UserResponse>(apiResponse.Results.ToString(), user);

			return user;
		}

	}
}
