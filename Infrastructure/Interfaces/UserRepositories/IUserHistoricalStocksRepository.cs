using Common.DTO.Stocks;
using Common.Entities.Users;

namespace Infrastructure.Repositories.UserRepositories
{
	public interface IUserHistoricalStocksRepository
	{
		public List<UserHistoricalStocks> GetAll(Guid userReference);
		public UserHistoricalStocks Add(RepositoryAddHistoricalStockRequest request);
	}
}
