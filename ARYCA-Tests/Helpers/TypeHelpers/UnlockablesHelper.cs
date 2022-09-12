using Common.DTO.Unlockables;
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

		public static UnlockableResponse GetUnlockableResponse()
		{
			return new UnlockableResponse
			{
				UnlockableReference = Guid.NewGuid(),
				Name = "Name",
				Type = "Type",
				Value = "Value",
				Unlocked = true,
				Price = 1,
				Display = true
			};
		}

		public static List<UnlockableResponse> GetUnlockableResponseList()
		{
			return new List<UnlockableResponse>()
			{
				GetUnlockableResponse()
			};
		}
	}
}
