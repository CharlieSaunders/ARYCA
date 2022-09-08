using Common.Data;
using Common.DTO.Users;
using Common.Entities.Users;
using Infrastructure.Repositories.UserRepositories;

namespace Infrastructure.InMemory.Users
{
	public class InMemoryUserRepository : IUserRepository
	{
		private readonly DataContext _db;
		public InMemoryUserRepository(DataContext db)
		{
			_db = db;
		}

		public List<User> Get()
		{
			return _db.Users.ToList();
		}

		public User Create(string username, string fname, string sname, string role)
		{
			var user = new User
			{
				Username = username,
				FirstName = fname,
				SecondName = sname,
				UserReference = Guid.NewGuid(),
				AvatarUrl = "basic-avatar.svg",
				Balance = 0,
				Theme = "default",
				Title = string.Empty,
				ParticleEffect = string.Empty,
				FontFamily = "'Rubik', sans-serif",
				Role = role
			};

			try
			{
				var userExists = GetByUsername(user.Username).IsValidUser();

				if (!userExists)
				{
					_db.Add(user);
					_db.SaveChanges();
					return user;
				}

				return new User();
			}
			catch
			{
				return new User();
			}
		}

		public User GetByUsername(string username)
		{
			try
			{
				return _db.Users.First(x => x.Username == username);
			}
			catch
			{
				return new User();
			}
		}

		public User GetByReference(Guid reference)
		{
			try
			{
				var user = _db.Users.FirstOrDefault(x => x.UserReference == reference);
				if (user is not null)
					return user;

				return new User();
			}
			catch
			{
				return new User();
			}
		}

		public bool Delete(Guid userReference)
		{
			try
			{
				var user = GetByReference(userReference);

				if (user.IsValidUser())
				{
					_db.Users.Remove(GetByReference(userReference));
					_db.SaveChanges();
					return true;
				}

				return false;

			}
			catch
			{
				return false;
			}
		}

		public User Update(RepositoryUpdateUserRequest updateUserRequest)
		{
			try
			{
				var userFromDb = GetByReference(updateUserRequest.UserReference);
				if (!userFromDb.IsValidUser())
					return new User();

				userFromDb.Username = updateUserRequest.Username;
				userFromDb.FirstName = updateUserRequest.FirstName;
				userFromDb.SecondName = updateUserRequest.SecondName;
				userFromDb.AvatarUrl = updateUserRequest.AvatarUrl;
				userFromDb.Balance = updateUserRequest.ResetBalance ? 0.0M : userFromDb.Balance;
				userFromDb.Theme = updateUserRequest.Theme;
				userFromDb.Title = updateUserRequest.Title;
				userFromDb.ParticleEffect = updateUserRequest.ParticleEffect;
				userFromDb.FontFamily = updateUserRequest.FontFamily;

				_db.SaveChanges();

				return GetByReference(updateUserRequest.UserReference);
			}
			catch
			{
				return new User();
			}
		}

		public void UpdateBalance(Guid userReference, decimal value)
		{
			var userFromDb = GetByReference(userReference);
			userFromDb.Balance += value;
			_db.SaveChanges();
		}
	}
}
