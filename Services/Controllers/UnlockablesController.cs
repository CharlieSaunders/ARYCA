using Common.Classes;
using Common.Data;
using Common.DTO.Unlockables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using Services.Routes;
using FromUri = System.Web.Http.FromUriAttribute;

namespace Services.Controllers
{
	[Route("public/[controller]")]
	public class UnlockablesController : ControllerBase
	{
		private readonly IUnlockablesService _unlockablesService;
		private readonly RequestHelper _requestHelper;

		public UnlockablesController(DataContext db)
		{
			_requestHelper = new RequestHelper(db);
			_unlockablesService = new UnlockablesService(db);
		}

		[HttpGet]
		[Authorize]
		public ActionResult<IServicesResponse> Get()
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _unlockablesService.GetAll(user), this);
		}

		[HttpGet("Unlocked")]
		[Authorize]
		public ActionResult<IServicesResponse> GetUnlocked()
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _unlockablesService.GetUnlocked(user), this);
		}

		[HttpPost]
		[Authorize]
		public ActionResult<IServicesResponse> Create([FromBody] CreateUnlockableRequest request, IFormFile image)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _unlockablesService.Create(user, request), this);
		}

		[HttpPut("Unlock/{unlockableReference}")]
		[Authorize]
		public ActionResult<IServicesResponse> Unlock([FromUri] Guid unlockableReference)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _unlockablesService.Unlock(user, unlockableReference), this);
		}

		[HttpPut("Unlock/Gift")]
		[Authorize]
		public ActionResult<IServicesResponse> Gift([FromBody] GiftUnlockableRequest request)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _unlockablesService.Gift(user, request), this);
		}

		[HttpPut("Update")]
		[Authorize]
		public ActionResult<IServicesResponse> Update([FromBody] UpdateUnlockableRequest request)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _unlockablesService.Update(request), this);
		}

		[HttpDelete("{reference}")]
		[Authorize]
		public ActionResult<IServicesResponse> DeleteItem([FromUri] Guid reference)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _unlockablesService.Delete(reference), this);
		}

		[HttpPatch("Populate")]
		[Authorize]
		public ActionResult<IServicesResponse> LockAllItems()
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _unlockablesService.Populate(), this);
		}


	}
}
