using Common.Entities.Pledges;

namespace Infrastructure.Interfaces
{
	public interface IPledgesRepository
	{
		public List<Pledge> GetAll();
		public Pledge GetByReference(Guid reference);
		public Pledge Create(string name, string details);
		public Pledge Update(Guid reference, string name, string details);
		public bool Delete(Guid reference);
	}
}
