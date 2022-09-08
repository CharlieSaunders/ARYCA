using Common.Classes;
using Common.Data;
using Common.DTO.Habits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using Services.Routes;
using FromUri = System.Web.Http.FromUriAttribute;

namespace Services.Controllers
{
	[Route("public/[controller]")]
	public class HabitsController : ControllerBase
	{
		private readonly IRequestHelper _requestHelper;
		private readonly IHabitsService _habitsService;
		public HabitsController(DataContext db)
		{
			_requestHelper = new RequestHelper(db);
			_habitsService = new HabitsService(db);
		}

		[HttpGet]
		[Authorize]
		public ActionResult<IServicesResponse> GetAll()
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _habitsService.GetAll(), this);
		}

		[HttpPost]
		[Authorize]
		public ActionResult<IServicesResponse> CreateHabit([FromBody] CreateHabitRequest request)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _habitsService.CreateHabit(request), this);
		}

		[HttpDelete("{reference}")]
		[Authorize]
		public ActionResult<IServicesResponse> DeleteHabit([FromUri] Guid reference)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _habitsService.DeleteHabit(reference), this);
		}

		[HttpGet("Assigned")]
		[Authorize]
		public ActionResult<IServicesResponse> AssignedHabits()
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _habitsService.GetAllAssigned(user.UserReference), this);
		}

		[HttpPut("Assign/{habitReference}")]
		[Authorize]
		public ActionResult<IServicesResponse> AssignHabit([FromUri] Guid habitReference)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _habitsService.AssignHabit(user.UserReference, habitReference), this);
		}

		[HttpPut("UnAssign/{habitReference}")]
		[Authorize]
		public ActionResult<IServicesResponse> UnAssignHabit([FromUri] Guid habitReference)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _habitsService.UnAssignHabit(user.UserReference, habitReference), this);
		}

		[HttpPost("Complete/{habitReference}")]
		[Authorize]
		public ActionResult<IServicesResponse> CompleteHabit([FromUri] Guid habitReference)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _habitsService.CompleteHabit(user.UserReference, habitReference), this);
		}

		[HttpPut]
		[Authorize]
		public ActionResult<IServicesResponse> UpdateHabit([FromBody] UpdateHabitRequest request)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _habitsService.UpdateHabit(request), this);
		}
	}
}
