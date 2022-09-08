using Common.Data;
using Common.DTO.Stocks;
using Common.Entities.Users;
using Infrastructure.Repositories.UserRepositories;

namespace Infrastructure.InMemory.Users
{
	public class InMemoryUserHistoricalStocksRepository : IUserHistoricalStocksRepository
	{
		private readonly DataContext _db;

		public InMemoryUserHistoricalStocksRepository(DataContext db)
		{
			_db = db;
		}

		public List<UserHistoricalStocks> GetAll(Guid userReference)
		{
			return _db.UserHistoricalStocks.Where(x => x.UserReference == userReference).OrderBy(x => x.Id).ToList();
		}

		public UserHistoricalStocks Add(RepositoryAddHistoricalStockRequest request)
		{
			var newHistoricalStock = new UserHistoricalStocks
			{
				UserReference = request.UserReference,
				Symbol = request.Symbol,
				Shares = request.Shares,
				Profit = request.Profit,
				Time = DateTime.Now.ToShortDateString()
			};

			var all = GetAll(request.UserReference);

			if (all.Count >= 100)
				_db.UserHistoricalStocks.RemoveRange(all);

			_db.UserHistoricalStocks.Add(newHistoricalStock);
			_db.SaveChanges();

			return newHistoricalStock;
		}
	}
}
