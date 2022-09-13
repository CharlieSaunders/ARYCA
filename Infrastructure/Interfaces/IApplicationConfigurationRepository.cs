using Common.Entities.Configuration;

namespace Infrastructure.Interfaces
{
	public interface IApplicationConfigurationRepository
	{
		public ApplicationConfiguration Get();
		public ApplicationConfiguration Update(string appName, DateTime date, string imageText, string imageUrl);
		public ApplicationConfiguration UpdateBalance(decimal change);
		public ApplicationConfiguration Create();
	}
}
