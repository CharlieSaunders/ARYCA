using System.ComponentModel.DataAnnotations;

namespace Common.DTO.Users
{
	public class UpdateUserRequest
	{
		[Required(ErrorMessage = "Username is required when updating a user.")]
		public string Username { get; set; }

		[Required(ErrorMessage = "First name is required when updating a user.")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Second name is required when updating a user.")]
		public string SecondName { get; set; }

		[Required(ErrorMessage = "Avatar URL is required when updating a user.")]
		public string AvatarUrl { get; set; }

		[Required(ErrorMessage = "Theme is required when updating a user.")]
		public string Theme { get; set; }

		[Required(ErrorMessage = "Title is required when updating a user.")]
		public string Title { get; set; }

		[Required(ErrorMessage = "Particle is required when updating a user.")]
		public string ParticleEffect { get; set; }

		[Required(ErrorMessage = "Font is required when updating a user.")]
		public string FontFamily { get; set; }

		public string Role { get; set; }

		public bool ResetBalance { get; set; }
	}
}