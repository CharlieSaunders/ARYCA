using Common.Entities.Users;
using System.Runtime.Serialization;

namespace Common.Classes.Users
{
	[DataContract]
	public class UserResponse
	{
		[DataMember(EmitDefaultValue = false)]
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
		public List<UserTransaction> Transactions { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public UserStats Stats { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Role { get; set; }

		public UserResponse()
		{
			UserReference = Guid.Empty;
			Transactions = new List<UserTransaction>();
			Stats = new UserStats();
		}

		public UserResponse(User userFromDb, List<UserTransaction> transactions, UserStats stats)
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
			Transactions = transactions;
			Stats = stats;
			ParticleEffect = userFromDb.ParticleEffect;
			FontFamily = userFromDb.FontFamily;
			Role = userFromDb.Role;
		}

		public UserResponse(User userFromDb)
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
			ParticleEffect = userFromDb.ParticleEffect;
			FontFamily = userFromDb.FontFamily;
			Role = userFromDb.Role;
		}

		public void AddAuthenticationToken(string token)
		{
			AuthenticationToken = token;
			HasActiveSession = true;
		}

		public bool IsValidUser()
		{
			return UserReference != Guid.Empty &&
				FirstName is not null &&
				SecondName is not null &&
				Username is not null;
		}
	}
}


