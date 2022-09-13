using Common.Entities.Logs;

namespace Infrastructure.Interfaces
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
}
