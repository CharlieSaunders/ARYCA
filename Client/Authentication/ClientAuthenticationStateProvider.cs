using Blazored.LocalStorage;
using Client.States;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Client.Authentication
{
	public class ClientAuthenticationStateProvider : AuthenticationStateProvider
	{
		private readonly ILocalStorageService _localStorage;
		private readonly UserState _userState;
		public ClientAuthenticationStateProvider(UserState userState, ILocalStorageService localStorage)
		{
			_userState = userState;
			_localStorage = localStorage;
		}
		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var authToken = await _localStorage.ContainKeyAsync("AuthenticationToken")
				? await _localStorage.GetItemAsStringAsync("AuthenticationToken")
				: string.Empty;

			var userReference = _userState.IsValidUser()
				? _userState.GetUserReference()
				: Guid.Empty;

			var currentState = new AuthenticationState(new ClaimsPrincipal());

			if (UserDetailsAreValid(userReference, authToken))
			{
				var identity = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.Name, authToken.Replace("\"", ""))
				}, "AuthenticationTokenClaim");

				currentState = new AuthenticationState(new ClaimsPrincipal(identity));
			}

			NotifyAuthenticationStateChanged(Task.FromResult(currentState));

			return currentState;
		}

		private static bool UserDetailsAreValid(Guid userReference, string authToken)
		{
			return userReference != Guid.Empty && authToken != string.Empty;
		}
	}
}
