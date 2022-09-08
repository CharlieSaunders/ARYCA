using System.ComponentModel.DataAnnotations;

namespace Common.DTO.Users
{
	public class CreateUserRequest
	{
		[Required]
		public string Username { get; set; }

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string SecondName { get; set; }

		[Required]
		public string Role { get; set; }

	}
}
