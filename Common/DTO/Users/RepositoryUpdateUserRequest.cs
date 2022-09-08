using Common.Entities.Users;

namespace Common.DTO.Users
{
	public class RepositoryUpdateUserRequest
	{
		public string Username { get; set; }
		public Guid UserReference { get; set; }
		public string FirstName { get; set; }
		public string SecondName { get; set; }
		public string AvatarUrl { get; set; }
		public bool ResetBalance { get; set; }
		public string Theme { get; set; }
		public string Title { get; set; }
		public string ParticleEffect { get; set; }
		public string FontFamily { get; set; }
		public string Role { get; set; }

		public RepositoryUpdateUserRequest()
		{

		}

		public RepositoryUpdateUserRequest(UpdateUserRequest request, User user)
		{
			UserReference = user.UserReference;
			Username = request.Username;
			FirstName = request.FirstName;
			SecondName = request.SecondName;
			AvatarUrl = request.AvatarUrl;
			ResetBalance = request.ResetBalance;
			Theme = request.Theme;
			Title = request.Title;
			ParticleEffect = request.ParticleEffect;
			FontFamily = request.FontFamily;
			Role = request.Role;
		}
	}

}
