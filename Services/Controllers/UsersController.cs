using Common.Classes;
using Common.Data;
using Common.DTO.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using Services.Routes;

namespace Services.Controllers
{
	[Route("public/[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly IUsersService _usersService;
		private readonly IRequestHelper _requestHelper;

		public UsersController(DataContext db, ITokenService tokenService)
		{
			_usersService = new UsersService(db, tokenService);
			_requestHelper = new RequestHelper(db);
		}

		[AllowAnonymous]
		[HttpPost("Session/{username}")]
		public ActionResult<IServicesResponse> CreateAuthenticationToken(string username)
		{
			var user = _requestHelper.UserFrom(username);
			return ServicesResponseHandler.From(user, _usersService.CreateSession(user), this);
		}

		[AllowAnonymous]
		[HttpPost]
		public ActionResult<IServicesResponse> Create([FromBody] CreateUserRequest request)
		{
			return AnonymousServicesResponseHandler.From(_usersService.CreateUser(request));
		}

		[HttpGet("{userReference}")]
		[Authorize]
		public ActionResult<IServicesResponse> GetUserByReference(Guid userReference)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _usersService.GetByReference(userReference), this);
		}

		[HttpPut]
		[Authorize]
		public ActionResult<IServicesResponse> UpdateUser([FromBody] UpdateUserRequest request)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _usersService.UpdateUserDetails(request, user), this);
		}

		[HttpGet("All")]
		[Authorize]
		public ActionResult<IServicesResponse> GetAll()
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _usersService.GetAll(), this);
		}
	}
}
