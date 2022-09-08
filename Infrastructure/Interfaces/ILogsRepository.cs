using Common.Data;
using Common.Entities.Logs;

namespace Infrastructure.Repositories
{
	public interface ILogsRepository
	{
		public List<Log> GetAll();
		public Log Get(string name, string description, string severity);
		public Log Update(int id);
		public Log Add(string name, string description, string severity, string area, string details);
		public bool Delete(int id);
		public Log GetById(int id);
	}

	public class LogsRepository : ILogsRepository
	{
		private readonly DataContext _db;
		public LogsRepository(DataContext db)
		{
			_db = db;
		}

		public List<Log> GetAll()
		{
			return _db.Logs.ToList();
		}

		public Log Get(string name, string description, string severity)
		{
			var log = _db.Logs.FirstOrDefault(x =>
				x.Name == name &&
				x.Description == description &&
				x.Severity == severity
				);

			if (log is not null)
				return log;
			else
				return new Log();
		}

		public Log Add(string name, string description, string severity, string area, string details)
		{
			var log = new Log
			{
				Name = name,
				Description = description,
				Details = details,
				Severity = severity,
				Area = area,
				FirstSeen = DateTime.Now.ToShortDateString(),
				LastSeen = DateTime.Now.ToShortDateString(),
				Occurances = 1
			};

			_db.Logs.Add(log);
			_db.SaveChanges();

			return log;
		}

		public Log Update(int id)
		{
			var log = _db.Logs.FirstOrDefault(x => x.Id == id);
			if (log is not null)
			{
				log.Occurances += 1;
				log.LastSeen = DateTime.Now.ToShortDateString();
				_db.SaveChanges();
				return log;
			}

			return new Log();
		}

		public bool Delete(int id)
		{
			var log = GetById(id);

			if (!log.IsEmpty())
			{
				_db.Logs.Remove(log);
				_db.SaveChanges();
				return true;
			}

			return false;
		}

		public Log GetById(int id)
		{
			var log = _db.Logs.FirstOrDefault(x => x.Id == id);
			if (log is not null)
				return log;

			return new Log();
		}
	}
}
