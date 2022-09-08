using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Common.Entities.Users
{
	[DataContract]
	public class User
	{
		[Key]
		[DataMember]
		public Guid UserReference { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Username { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string? AuthenticationToken { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string FirstName { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string SecondName { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string AvatarUrl { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public bool HasActiveSession { get; set; } = false;

		[DataMember(EmitDefaultValue = true)]
		public decimal Balance { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Theme { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Title { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string ParticleEffect { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string FontFamily { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Role { get; set; }

		public User()
		{
			UserReference = Guid.Empty;
		}

		public User(User userFromDb)
		{
			UserReference = userFromDb.UserReference;
			Username = userFromDb.Username;
			AvatarUrl = userFromDb.AvatarUrl;
			FirstName = userFromDb.FirstName;
			SecondName = userFromDb.SecondName;
			HasActiveSession = userFromDb.UserReference != Guid.Empty;
			Balance = userFromDb.Balance;
			Theme = userFromDb.Theme;
			Title = userFromDb.Title;
			FontFamily = userFromDb.FontFamily;
			ParticleEffect = userFromDb.ParticleEffect;
			Role = userFromDb.Role;
		}

		public void AddAuthenticationToken(string token)
		{
			AuthenticationToken = token;
			HasActiveSession = true;
		}

		public bool IsValidReference()
		{
			return UserReference != Guid.Empty;
		}

		public bool IsValidUser()
		{
			return UserReference != Guid.Empty &&
				FirstName is not null &&
				SecondName is not null &&
				Username is not null;
		}

		public bool CanPurchase(decimal cost)
		{
			return Balance - cost >= 0;
		}

		public struct UserRole
		{
			public const string Admin = "Admin";
			public const string Standard = "Standard";
		}
	}
}