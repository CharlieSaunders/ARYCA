using Blazored.LocalStorage;

namespace Client.Helpers
{
	public static class LocalStorageHelper
	{
		public async static Task<string> GetAuthToken(ILocalStorageService localStorage)
		{
			var jwt = await localStorage.ContainKeyAsync("AuthenticationToken")
				? await localStorage.GetItemAsStringAsync("AuthenticationToken")
				: String.Empty;

			return jwt.Replace("\"", "");
		}
	}
}
