using Common.Data;
using Common.Entities.Unlockables;
using Infrastructure.Interfaces;

namespace Infrastructure.EF.Unlockables
{
	public class EFUnlockablesRepository : IUnlockablesRepository
	{
		private readonly DataContext _db;

		public EFUnlockablesRepository(DataContext db)
		{
			_db = db;
		}

		public List<Unlockable> GetAll()
		{
			return _db.Unlockables.ToList();
		}

		public Unlockable GetByName(string name)
		{
			var unlockable = _db.Unlockables.FirstOrDefault(x => x.Name == name);
			if (unlockable is not null)
				return unlockable;

			return new Unlockable();
		}

		public Unlockable Create(string name, string type, decimal price, string value, bool display)
		{
			var newUnlockable = new Unlockable
			{
				Reference = Guid.NewGuid(),
				Name = name,
				Type = type,
				Price = price,
				Value = value,
				Display = display
			};

			try
			{
				_db.Unlockables.Add(newUnlockable);
				_db.SaveChanges();
				return newUnlockable;
			}
			catch
			{
				return new Unlockable();
			}
		}

		public Unlockable GetByReference(Guid reference)
		{
			var unlockable = _db.Unlockables.FirstOrDefault(x => x.Reference == reference);
			if (unlockable is not null)
				return unlockable;

			return new Unlockable();
		}

		public bool Delete(Unlockable item)
		{
			try
			{
				_db.Unlockables.Remove(item);
				_db.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}

		}

		public Unlockable Update(Guid reference, string name, string type, decimal price, string value, bool display)
		{
			try
			{
				var item = GetByReference(reference);
				item.Name = name;
				item.Type = type;
				item.Price = price;
				item.Value = value;
				item.Display = display;
				_db.SaveChanges();

				return item;
			}
			catch
			{
				return new Unlockable();
			}

		}
	}
}
