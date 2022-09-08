using Common.Classes;
using Common.Data;
using Common.DTO.Configs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using Services.Routes;
using FromUri = System.Web.Http.FromUriAttribute;

namespace Services.Controllers
{
	[Route("public/[controller]")]
	public class ConfigController : ControllerBase
	{
		private readonly IRequestHelper _requestHelper;
		private readonly IConfigService _configService;
		private readonly ILogsService _logsService;

		public ConfigController(DataContext db)
		{
			_requestHelper = new RequestHelper(db);
			_configService = new ConfigService(db);
			_logsService = new LogsService(db);
		}

		[AllowAnonymous]
		[HttpGet("Health")]
		public ActionResult<IServicesResponse> HealthCheck()
		{
			return AnonymousServicesResponseHandler.From(new IServicesResponse("Healthy"));
		}

		[HttpGet("Application")]
		[Authorize]
		public ActionResult<IServicesResponse> GetAppConfig()
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _configService.GetAppConfig(), this);
		}

		[HttpPut("Application")]
		[Authorize]
		public ActionResult<IServicesResponse> UpdateAppConfig([FromBody] UpdateAppConfigRequest request)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _configService.Update(request), this);
		}

		[HttpGet("Logs")]
		[Authorize]
		public ActionResult<IServicesResponse> GetAllLogs()
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _logsService.Get(), this);
		}

		[HttpDelete("Logs/{id}")]
		[Authorize]
		public ActionResult<IServicesResponse> DeleteLog([FromUri] int id)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _logsService.Delete(id), this);
		}
	}
}
