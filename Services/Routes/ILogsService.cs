using Common.Classes;
using Common.Data;
using Common.DTO.Logs;
using Common.Entities.Logs;
using Infrastructure.Repositories;

namespace Services.Routes
{
	public interface ILogsService
	{
		public IServicesResponse Add(CreateLogRequest request);
		public IServicesResponse Get();
		public IServicesResponse Delete(int id);
	}

	public class LogsService : ILogsService
	{
		private readonly ILogsRepository _logsRepository;

		public LogsService(DataContext db)
		{
			_logsRepository = new LogsRepository(db);
		}

		public IServicesResponse Get() => new(_logsRepository.GetAll());

		public IServicesResponse Add(CreateLogRequest request)
		{
			var response = new IServicesResponse(new Log());
			var logExists = _logsRepository.Get(request.Name, request.Description, request.Severity);
			if (!logExists.IsEmpty())
				response.Results = _logsRepository.Update(logExists.Id);
			else
				response.Results = _logsRepository.Add(request.Name, request.Description, request.Severity, request.Area, request.Details);

			return response;
		}

		public IServicesResponse Delete(int id)
		{
			var response = new IServicesResponse(false);
			var logExists = _logsRepository.GetById(id);
			if (!logExists.IsEmpty())
				response.Results = _logsRepository.Delete(id);

			return response;
		}
	}
}
