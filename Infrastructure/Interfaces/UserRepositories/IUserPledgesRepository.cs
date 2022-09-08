using Common.DTO.Pledges;
using Common.Entities.Users;

namespace Infrastructure.Repositories.UserRepositories
{
	public interface IUserPledgesRepository
	{
		public UserPledges Add(RepositoryAddPledgeRequest request);
		public List<UserPledges> GetAll(Guid userReference);
		public UserPledges Get(int pledgeId);
		public UserPledges Update(UserPledges pledge);
		public void Delete(int pledgeId);
	}
}
