using Common.Entities.Unlockables;

namespace ARYCA_Tests.Helpers.TypeHelpers
{
	public static class UnlockablesHelper
	{
		public static Unlockable GetGenericUnlockable()
		{
			return new Unlockable
			{
				Reference = Guid.Parse("cd2271b6-de15-0000-0000-9a40a3e6106d"),
				Type = UnlockableTypes.Title,
				Name = "Unlockable Title",
				Price = 10,
				Value = "Title"
			};
		}
	}
}
