using Common.Entities.Unlockables;

namespace Infrastructure.Interfaces
{
	public interface IUnlockablesRepository
	{
		List<Unlockable> GetAll();
		Unlockable GetByName(string name);
		Unlockable Create(string name, string type, decimal price, string value, bool display);
		Unlockable GetByReference(Guid reference);
		Unlockable Update(Guid reference, string name, string type, decimal price, string value, bool display);
		bool Delete(Unlockable item);
	}
}
