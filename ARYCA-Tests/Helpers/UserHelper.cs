using Common.Entities.Users;
using static Common.Entities.Users.User;

namespace ARYCA_Tests.Helpers
{
	public static class UserHelper
	{

		public static User GetActiveUser()
		{
			return new User
			{
				UserReference = Guid.NewGuid(),
				Username = "One",
				FirstName = "First",
				SecondName = "User1",
				AvatarUrl = "Test.svg",
				Balance = 100.00M,
				Theme = "Default",
				Title = "Mr",
				ParticleEffect = "ParticleEffect",
				FontFamily = "FontFamily",
				HasActiveSession = true,
				Role = UserRole.Admin
			};
		}

		public static User GetInactiveUser(string userInfo, int genericUserCount)
		{
			return new User
			{
				UserReference = Guid.Parse($"cd2271b6-de15-000{genericUserCount}-a940-9a40a3e6106d"),
				Username = $"{userInfo} username",
				FirstName = $"{userInfo} Firstname",
				SecondName = $"{userInfo} Secondname",
				AvatarUrl = $"{userInfo} Avatar",
				Balance = 100.00M,
				Theme = $"{userInfo} Theme",
				Title = $"{userInfo} Title",
				ParticleEffect = "ParticleEffect",
				FontFamily = "FontFamily",
				HasActiveSession = false,
				Role = UserRole.Standard
			};
		}
	}
}
