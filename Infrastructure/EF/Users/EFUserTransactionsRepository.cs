using Common.Data;
using Common.Entities.Users;
using Infrastructure.Repositories.UserRepositories;

namespace Infrastructure.EF.Users
{
	public class InMemoryUserTransactionRepository : IUserTransactionsRepository
	{
		private readonly DataContext _db;

		public InMemoryUserTransactionRepository(DataContext db)
		{
			_db = db;
		}

		public List<UserTransaction> GetAllByReference(Guid userReference)
		{
			return _db.UserTransactions.Where(x => x.UserReference == userReference).OrderByDescending(x => x.Id).ToList();
		}

		public UserTransaction Add(Guid userReference, string type, decimal value)
		{
			var allTransactions = GetAllByReference(userReference);
			try
			{
				if (allTransactions.Count >= 15)
					_db.Remove(allTransactions[allTransactions.Count - 1]);

				var newTransaction = new UserTransaction();
				newTransaction.UserReference = userReference;
				newTransaction.Value = value;
				newTransaction.Type = type;
				newTransaction.Added = DateTime.Now;
				_db.UserTransactions.Add(newTransaction);
				_db.SaveChanges();
				return newTransaction;
			}
			catch
			{
				return new UserTransaction();
			}

		}
	}
}
