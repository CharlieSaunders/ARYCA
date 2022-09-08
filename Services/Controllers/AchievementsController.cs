using Common.Classes;
using Common.Data;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using Services.Routes;
using FromUri = System.Web.Http.FromUriAttribute;

namespace Services.Controllers
{
	[Route("Public/[Controller]")]
	public class AchievementsController : ControllerBase
	{
		private readonly IRequestHelper _requestHelper;
		private readonly IAchievementService _achievementService;

		public AchievementsController(DataContext db)
		{
			_requestHelper = new RequestHelper(db);
			_achievementService = new AchievementService(db);
		}

		[HttpGet]
		public ActionResult<IServicesResponse> GetAll()
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _achievementService.GetAll(), this);
		}

		[HttpGet("User")]
		public ActionResult<IServicesResponse> GetAllForUser()
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _achievementService.GetForUser(user.UserReference), this);
		}

		[HttpPut("User/{achievementId}")]
		public ActionResult<IServicesResponse> UnlockAchievement([FromUri] int achievementId)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _achievementService.Unlock(user.UserReference, achievementId), this);
		}
	}
}
