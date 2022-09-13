using Common.Classes;
using Common.Classes.ErrorHandling;
using Common.Classes.Users;
using Common.Data;
using Common.DTO.Logs;
using Common.DTO.Users;
using Common.Entities.Logs;
using Common.Entities.Users;
using Infrastructure.EF;
using Infrastructure.EF.Investments;
using Infrastructure.Interfaces;
using Services.Helpers;

namespace Services.Routes
{
	public interface IUsersService
	{
		IServicesResponse GetByUsername(string username);
		IServicesResponse GetByReference(Guid userReference);
		IServicesResponse CreateSession(User user);
		IServicesResponse GetAll();
		IServicesResponse UpdateUserDetails(UpdateUserRequest request, User user);
		IServicesResponse CreateUser(CreateUserRequest request);

		User UserMapper(User usersData, bool createToken);
		string GetAuthenticationToken(Guid userReference, bool createToken);
	}

	public class UsersService : IUsersService
	{
		private readonly EFUsersRepositories _inMemoryUserRepository;
		private readonly IInvestmentRepository _investmentRepository;
		private readonly ITokenService _tokenService;
		private readonly ILogsService _logsService;

		public UsersService(DataContext db, ITokenService tokenService)
		{
			_inMemoryUserRepository = new EFUsersRepositories(db);
			_tokenService = tokenService;
			_investmentRepository = new EFInvestmentRepository(db);
			_logsService = new LogsService(db);
		}

		public string GetAuthenticationToken(Guid userReference, bool createToken) => createToken ? _tokenService.CreateToken(userReference) : string.Empty;

		public IServicesResponse GetByUsername(string username)
		{
			var response = new IServicesResponse(new User());
			try
			{
				var user = _inMemoryUserRepository.GetUserByUsername(username);
				response.Results = UserMapper(user, false);
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to get by username", this.GetType()));
			}
			return response;
		}

		public IServicesResponse CreateSession(User user)
		{
			var response = new IServicesResponse(user);
			try
			{
				if (!user.IsValidReference())
				{
					response.AddError(Error.Users.NoUsersFound, "User reference not valid");
					return response;
				}

				if (!user.IsValidUser())
				{
					response.AddError(Error.Users.NoUsersFound, "User found was not valid");
					_logsService.Add(new CreateLogRequest(this.GetType().ToString(), "User was not valid"));
					return response;
				}

				response.Results.AddAuthenticationToken(GetAuthenticationToken(user.UserReference, true));
			}
			catch (Exception ex)
			{
				response.AddError(ex.Message, Error.Users.ServicesUnableToCreateSession);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, Error.Users.ServicesUnableToCreateSession, this.GetType()));
			}

			return response;
		}

		public IServicesResponse UpdateUserDetails(UpdateUserRequest request, User user)
		{
			var response = new IServicesResponse(user);
			try
			{
				response.Results = UserMapper(_inMemoryUserRepository.UpdateUser(new RepositoryUpdateUserRequest(request, user)), false);
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to update user details", this.GetType()));
			}

			return response;
		}

		public IServicesResponse GetByReference(Guid userReference)
		{
			var response = new IServicesResponse(new UserResponse());
			try
			{
				var user = _inMemoryUserRepository.GetUserByReference(userReference);
				if (user is not null && user.IsValidReference())
					response.Results = new UserResponse(user, _inMemoryUserRepository.GetAllTransactionsByReference(userReference), _inMemoryUserRepository.GetStatByReference(userReference));
				else
					response.AddError(Error.Users.NoUsersFound, $"Unable to find user with reference '{userReference}'");
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to get user by reference", this.GetType()));
			}

			return response;
		}

		public IServicesResponse CreateUser(CreateUserRequest request)
		{
			var response = new IServicesResponse(new User());
			try
			{
				response.Results = _inMemoryUserRepository.CreateUser(request.Username, request.FirstName, request.SecondName, request.Role);
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to create user", this.GetType()));
			}

			return response;
		}

		public IServicesResponse GetAll()
		{
			var response = new IServicesResponse(new List<SlimUser>());
			try
			{
				var allUsers = _inMemoryUserRepository.GetAllUsers();
				foreach (var user in allUsers)
				{
					var pledges = _inMemoryUserRepository.GetAllPledges(user.UserReference);
					var ownedPledges = pledges.Where(x => x.AssignerReference == user.UserReference).ToList();
					var habits = _inMemoryUserRepository.GetHabits(user.UserReference);
					var stocks = _investmentRepository.GetAllForUser(user.UserReference);
					response.Results.Add(new SlimUser(user, habits.Count, ownedPledges.Count, stocks.Count));
				}

			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to get all users", this.GetType()));
			}

			return response;
		}

		public User UserMapper(User usersData, bool createToken)
		{
			if (createToken)
				usersData.AuthenticationToken = _tokenService.CreateToken(usersData.UserReference);

			return usersData;
		}
	}
}
