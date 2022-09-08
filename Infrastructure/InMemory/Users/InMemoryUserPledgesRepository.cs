using Common.Data;
using Common.DTO.Pledges;
using Common.Entities.Users;
using Infrastructure.Repositories.UserRepositories;

namespace Infrastructure.InMemory.Users
{
	public class InMemoryUserPledgesRepository : IUserPledgesRepository
	{
		private readonly DataContext _db;

		public InMemoryUserPledgesRepository(DataContext db)
		{
			_db = db;
		}

		public UserPledges Add(RepositoryAddPledgeRequest request)
		{
			var newPledge = new UserPledges
			{
				AssignerReference = request.AssignerReference,
				PledgeReference = request.PledgeReference,
				Value = request.Value,
				AdditionalInformation = request.AdditionalDetails,
				AssigneeReference = request.AssigneeReference
			};

			try
			{
				_db.UserPledges.Add(newPledge);
				_db.SaveChanges();
				return newPledge;
			}
			catch
			{
				return new UserPledges();
			}
		}

		public List<UserPledges> GetAll(Guid userReference)
		{
			var response = new List<UserPledges>();

			response.AddRange(_db.UserPledges.Where(x => x.AssigneeReference == userReference).ToList());
			response.AddRange(_db.UserPledges.Where(x => x.AssignerReference == userReference).ToList());

			return response;
		}

		public UserPledges Get(int pledgeId)
		{
			try
			{
				var userPledges = _db.UserPledges.FirstOrDefault(x => x.Id == pledgeId);
				if (userPledges is not null)
					return userPledges;


				return new UserPledges();
			}
			catch
			{
				return new UserPledges();
			}
		}

		public UserPledges Update(UserPledges pledge)
		{

			try
			{
				var currentPledge = _db.UserPledges.FirstOrDefault(x => x.Id == pledge.Id);
				if (currentPledge is not null)
				{
					currentPledge = pledge;
					_db.SaveChanges();
					return currentPledge;
				}

				return new UserPledges();
			}
			catch
			{
				return new UserPledges();
			}
		}

		public void Delete(int pledgeId)
		{

			var pledge = Get(pledgeId);
			if (pledge.PledgeReference != Guid.Empty)
			{
				_db.UserPledges.Remove(pledge);
				_db.SaveChanges();
			}
		}
	}
}
