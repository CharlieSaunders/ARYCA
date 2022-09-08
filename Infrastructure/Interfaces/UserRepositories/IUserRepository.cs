using Common.DTO.Users;
using Common.Entities.Users;

namespace Infrastructure.Repositories.UserRepositories
{
	public interface IUserRepository
	{
		User GetByReference(Guid reference);
		List<User> Get();
		User GetByUsername(string username);
		User Update(RepositoryUpdateUserRequest updateUserRequest);
		bool Delete(Guid userReference);
		User Create(string username, string fname, string sname, string role);
		void UpdateBalance(Guid userReference, decimal value);
	}
}
