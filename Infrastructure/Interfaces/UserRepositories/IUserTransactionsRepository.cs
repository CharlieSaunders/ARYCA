using Common.Entities.Users;

namespace Infrastructure.Repositories.UserRepositories
{
	public interface IUserTransactionsRepository
	{
		public UserTransaction Add(Guid userReference, string type, decimal value);
		public List<UserTransaction> GetAllByReference(Guid userReference);
	}
}
