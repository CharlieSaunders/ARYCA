using Common.Data;
using Common.Entities.Pledges;

namespace Infrastructure.Repositories
{
	public interface IPledgesRepository
	{
		public List<Pledge> GetAll();
		public Pledge GetByReference(Guid reference);
		public Pledge Create(string name, string details);
		public Pledge Update(Guid reference, string name, string details);
		public bool Delete(Guid reference);
	}

	public class PledgesRepository : IPledgesRepository
	{
		private readonly DataContext _db;

		public PledgesRepository(DataContext db)
		{
			_db = db;
		}

		public Pledge GetByReference(Guid reference)
		{
			var pledge = _db.Pledges.FirstOrDefault(x => x.Reference == reference);
			if (pledge is not null)
				return pledge;

			return new Pledge();
		}

		public Pledge Create(string name, string details)
		{
			try
			{
				var newPledge = new Pledge
				{
					Name = name,
					Details = details,
					Reference = Guid.NewGuid()
				};

				_db.Pledges.Add(newPledge);
				_db.SaveChanges();
				return newPledge;
			}
			catch
			{
				return new Pledge();
			}
		}

		public bool Delete(Guid reference)
		{
			try
			{
				var pledge = GetByReference(reference);
				if (pledge is not null)
				{
					_db.Pledges.Remove(pledge);
					_db.SaveChanges();
					return true;
				}

				return false;
			}
			catch
			{
				return false;
			}

		}

		public List<Pledge> GetAll()
		{
			return _db.Pledges.ToList();
		}

		public Pledge Update(Guid reference, string name, string details)
		{
			try
			{
				var pledge = GetByReference(reference);
				if (pledge is not null)
				{
					pledge.Name = name;
					pledge.Details = details;
					_db.SaveChanges();
					return pledge;
				}

				return new Pledge();
			}
			catch
			{
				return new Pledge();
			}

		}
	}
}
