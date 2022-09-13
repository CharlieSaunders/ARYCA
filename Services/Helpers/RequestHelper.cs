using Common.Classes.ErrorHandling;
using Common.Data;
using Common.Entities.Users;
using Infrastructure.EF;
namespace Services.Helpers
{
	public interface IRequestHelper
	{
		User UserFrom(HttpRequest request);
		User UserFrom(string username);
	}

	public class RequestHelper : IRequestHelper
	{
		private readonly string JWT_TOKEN_NAME = @"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
		private readonly EFUsersRepositories _usersRepository;

		public RequestHelper(DataContext db)
		{
			_usersRepository = new EFUsersRepositories(db);
		}

		public User UserFrom(HttpRequest request)
		{
			var usersList = new List<User>();
			var requestHeaders = new Dictionary<string, string>();

			foreach (var header in request.Headers)
				requestHeaders.Add(header.Key, header.Value);

			if (requestHeaders.ContainsKey("X-ARYCA-UserReference"))
			{
				var reference = requestHeaders["X-ARYCA-UserReference"];
				var userFromDb = _usersRepository.GetUserByReference(Guid.Parse(reference));
				usersList.Add(new User(userFromDb));
			}

			if (requestHeaders.ContainsKey("Authorization"))
			{
				var reference = request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == JWT_TOKEN_NAME)?.Value;

				if (reference is not null)
				{
					var userFromDb = _usersRepository.GetUserByReference(Guid.Parse(reference));
					usersList.Add(new User(userFromDb));
				}
			}

			if (usersList.Count == 0)
				throw new Exception(Error.Users.NoUsersFound);

			if (usersList.Count == 1)
			{
				if (usersList.First().HasActiveSession)
					return usersList.First();
				else
					throw new Exception(Error.Users.NoActiveSession);
			}

			if (!UsersMatch(usersList))
				throw new Exception(Error.Users.UsersFoundDoNotMatch);

			if (!usersList.First().HasActiveSession)
				throw new Exception(Error.Users.NoActiveSession);

			return usersList.First();
		}

		public User UserFrom(string username)
		{
			try
			{
				return _usersRepository.GetUserByUsername(username);
			}
			catch
			{
				return new User();
			}
		}

		private static bool UsersMatch(List<User> usersList) =>
			usersList[0].Username == usersList[1].Username &&
			usersList[0].UserReference == usersList[1].UserReference;
	}
}
