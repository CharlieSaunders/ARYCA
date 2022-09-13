using Common.Data;
using Common.Entities.Users;
using Infrastructure.Repositories.UserRepositories;

namespace Infrastructure.EF.Users
{
	public class EFUserUnlockablesRepository : IUserUnlockablesRepository
	{
		private readonly DataContext _db;

		public EFUserUnlockablesRepository(DataContext db)
		{
			_db = db;
		}

		public List<UserUnlockables> GetAll(Guid userReference)
		{
			return _db.UserUnlockables.Where(x => x.UserReference == userReference).ToList();
		}

		public bool Add(Guid userReference, Guid unlockReference)
		{
			try
			{
				_db.UserUnlockables.Add(new UserUnlockables
				{
					UserReference = userReference,
					UnlockableReference = unlockReference
				});

				_db.SaveChanges();

				return true;
			}
			catch
			{
				return false;
			}
		}

		public UserUnlockables Get(Guid userReference, Guid unlockReference)
		{
			var unlockables = _db.UserUnlockables.FirstOrDefault(x => x.UserReference == userReference && x.UnlockableReference == unlockReference);
			if (unlockables is not null)
				return unlockables;

			return new UserUnlockables();
		}

		public bool DeleteAll(Guid reference)
		{
			var all = _db.UserUnlockables.Where(x => x.UnlockableReference == reference).ToList();
			_db.UserUnlockables.RemoveRange(all);
			_db.SaveChanges();
			return true;
		}

		public bool DeleteAllUnlocks()
		{
			var all = _db.UserUnlockables.ToList();
			_db.UserUnlockables.RemoveRange(all);
			_db.SaveChanges();
			return true;
		}
	}

}
