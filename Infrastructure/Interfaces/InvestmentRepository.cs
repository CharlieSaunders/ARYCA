using Common.Data;
using Common.Entities.Investments;

namespace Infrastructure.Repositories
{
	public interface IInvestmentRepository
	{
		public List<UserInvestments> GetAllForUser(Guid userReference);
		public UserInvestments Add(Guid userReference, string symbol, decimal share, decimal purchasePrice);
		public UserInvestments Sell(Guid userReference, decimal shares, int id, decimal newSellPrice);
	}

	public class InvestmentRepository : IInvestmentRepository
	{
		private readonly DataContext _db;

		public InvestmentRepository(DataContext db)
		{
			_db = db;
		}

		public List<UserInvestments> GetAllForUser(Guid userReference)
		{
			return _db.UserInvestments.Where(x => x.UserReference == userReference).ToList();
		}

		public UserInvestments Add(Guid userReference, string symbol, decimal share, decimal purchasePrice)
		{
			var newInvestment = new UserInvestments
			{
				UserReference = userReference,
				Symbol = symbol,
				Share = share,
				Price = purchasePrice
			};

			try
			{
				_db.UserInvestments.Add(newInvestment);
				_db.SaveChanges();
				return newInvestment;
			}
			catch
			{
				return new UserInvestments();
			}
		}

		public UserInvestments Sell(Guid userReference, decimal shares, int id, decimal newSellPrice)
		{
			var currentValue = _db.UserInvestments.FirstOrDefault(x => x.Id == id);
			if (currentValue is not null)
			{

				var shouldDelete = currentValue.Share - shares == 0;
				if (shouldDelete)
				{
					_db.UserInvestments.Remove(currentValue);
					_db.SaveChanges();
					return currentValue;
				}

				currentValue.Share -= shares;
				currentValue.Price = newSellPrice;
				_db.SaveChanges();
				return currentValue;
			}
			else
				return new UserInvestments();

		}
	}
}
