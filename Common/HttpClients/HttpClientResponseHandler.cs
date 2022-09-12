using Common.Classes;
using Newtonsoft.Json;

namespace Common.HttpClients
{
	public static class HttpClientResponseHandler
	{
		public async static Task<IServicesResponse> From(HttpResponseMessage apiResponse)
		{
			var response = new IServicesResponse();
			var apiContent = await apiResponse.Content.ReadAsStringAsync();

			var deserialized = JsonConvert.DeserializeObject<IServicesResponse>(apiContent);
			if (deserialized is not null)
				response = deserialized;
			else
				response.AddError("Error in response", "Response returned was null");

			return response;
		}
	}
}

