using Common.Classes;
using Common.Data;
using Common.DTO.Configs;
using Common.DTO.Logs;
using Common.Entities.Configuration;
using Common.Entities.Logs;
using Infrastructure.EF.AppConfig;
using Infrastructure.Interfaces;

namespace Services.Routes
{
	public interface IConfigService
	{
		public IServicesResponse GetAppConfig();
		public IServicesResponse Update(UpdateAppConfigRequest request);
	}

	public class ConfigService : IConfigService
	{
		private readonly IApplicationConfigurationRepository _appConfigRepository;
		private readonly ILogsService _logsService;

		public ConfigService(DataContext db)
		{
			_appConfigRepository = new EFAppConfigurationRepository(db);
			_logsService = new LogsService(db);
		}

		public IServicesResponse GetAppConfig()
		{
			var response = new IServicesResponse(new ApplicationConfiguration());
			try
			{
				var config = _appConfigRepository.Get();
				if (!config.IsValid())
					config = _appConfigRepository.Create();

				response.Results = config;
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to get application config", this.GetType()));
			}

			return response;
		}

		public IServicesResponse Update(UpdateAppConfigRequest request)
		{
			var response = new IServicesResponse(new ApplicationConfiguration());
			try
			{
				response.Results = _appConfigRepository.Update(request.AppName, request.Date, request.ImageText, request.ImageUrl);
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to update application config", this.GetType()));
			}

			return response;
		}
	}
}
