using Common.Classes;
using Common.Data;
using Common.DTO.Pledges;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using Services.Routes.Pledges;
using FromUri = System.Web.Http.FromUriAttribute;

namespace Services.Controllers
{
	[Route("public/[controller]")]
	public class PledgesController : ControllerBase
	{
		private readonly RequestHelper _requestHelper;
		private readonly IPledgesService _pledgesService;

		public PledgesController(DataContext db)
		{
			_requestHelper = new RequestHelper(db);
			_pledgesService = new PledgesService(db);
		}

		[HttpGet]
		[Authorize]
		public ActionResult<IServicesResponse> GetAll()
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _pledgesService.GetAll(), this);
		}

		[HttpPost]
		[Authorize]
		public ActionResult<IServicesResponse> Create([FromBody] CreatePledgeRequest request)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _pledgesService.Create(request), this);
		}

		[HttpPut]
		[Authorize]
		public ActionResult<IServicesResponse> Update([FromBody] UpdatePledgeRequest request)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _pledgesService.Update(request), this);
		}

		[HttpDelete("{pledgeReference}")]
		[Authorize]
		public ActionResult<IServicesResponse> Delete([FromUri] Guid pledgeReference)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _pledgesService.Delete(pledgeReference), this);
		}

		[HttpPut("Assign")]
		[Authorize]
		public ActionResult<IServicesResponse> Assign([FromBody] AssignPledgeRequest request)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _pledgesService.Assign(user, request), this);
		}

		[HttpGet("Assigned")]
		[Authorize]
		public ActionResult<IServicesResponse> Assigned()
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _pledgesService.Assigned(user), this);
		}

		[HttpPut("Assigned/Status")]
		[Authorize]
		public ActionResult<IServicesResponse> UpdateAssignedStatus([FromBody] UpdatePledgeStatusRequest request)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _pledgesService.UpdateAssignedStatus(user, request), this);
		}
	}
}
