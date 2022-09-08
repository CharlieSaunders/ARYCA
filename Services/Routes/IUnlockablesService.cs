using Common.Classes;
using Common.Classes.ErrorHandling;
using Common.Data;
using Common.DTO.Logs;
using Common.DTO.Unlockables;
using Common.Entities.Logs;
using Common.Entities.Unlockables;
using Common.Entities.Users;
using Infrastructure.InMemory;
using Infrastructure.Repositories;

namespace Services.Routes
{
	public interface IUnlockablesService
	{
		public IServicesResponse GetAll(User user);
		public IServicesResponse GetUnlocked(User user);
		public IServicesResponse Create(User user, CreateUnlockableRequest request);
		public IServicesResponse Unlock(User user, Guid unlockableReference);
		public IServicesResponse Gift(User user, GiftUnlockableRequest request);
		public IServicesResponse Delete(Guid reference);
		public IServicesResponse Update(UpdateUnlockableRequest request);
		public IServicesResponse Populate();
	}

	public class UnlockablesService : IUnlockablesService
	{
		private readonly InMemoryUserRepositories _inMemoryUserRepository;
		private readonly IUnlockablesRepository _unlockablesRepository;
		private readonly IApplicationConfigurationRepository _appConfigRepository;
		private readonly ILogsService _logsService;

		private readonly Dictionary<string, string> FOLDER_PATHS = new()
		{
			{"UserSvgs", @$"C:\WebServerRoot\Demo\ARYCA\Client\wwwroot\assets\userSvgs\" },
		};

		public UnlockablesService(DataContext db)
		{
			_inMemoryUserRepository = new InMemoryUserRepositories(db);
			_unlockablesRepository = new UnlockablesRepository(db);
			_appConfigRepository = new ApplicationConfigurationRepository(db);
			_logsService = new LogsService(db);
		}

		public IServicesResponse GetAll(User user)
		{
			var response = new IServicesResponse(new List<UnlockableResponse>());
			try
			{
				var allItems = _unlockablesRepository.GetAll();
				var unlocked = _inMemoryUserRepository.GetAllUnlockables(user.UserReference);
				var unlockedItems = unlocked.Select(x => x.UnlockableReference).ToList();
				foreach (var item in allItems)
				{
					response.Results.Add(new UnlockableResponse
					{
						UnlockableReference = item.Reference,
						Name = item.Name,
						Type = item.Type,
						Value = item.Value,
						Unlocked = unlockedItems.Contains(item.Reference),
						Price = item.Price,
						Display = item.Display,
					});
				}

			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable get all unlockables", this.GetType()));
			}

			return response;
		}

		public IServicesResponse GetUnlocked(User user)
		{
			var response = new IServicesResponse(new List<UnlockableResponse>());
			try
			{
				var unlocked = _inMemoryUserRepository.GetAllUnlockables(user.UserReference);
				var unlockedItemsToGet = unlocked.Select(x => x.UnlockableReference).ToList();
				var allItems = _unlockablesRepository.GetAll();

				foreach (var unlockedItemReference in unlockedItemsToGet)
				{
					var matchingItem = allItems.First(x => x.Reference == unlockedItemReference);
					response.Results.Add(new UnlockableResponse
					{
						UnlockableReference = matchingItem.Reference,
						Name = matchingItem.Name,
						Type = matchingItem.Type,
						Value = matchingItem.Value,
						Display = matchingItem.Display
					});
				}

			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable get all unlocks", this.GetType()));
			}

			return response;
		}

		public IServicesResponse Create(User user, CreateUnlockableRequest request)
		{
			var response = new IServicesResponse(new Unlockable());
			try
			{
				var unlockable = _unlockablesRepository.GetByName(request.Name);

				if (unlockable.IsEmpty())
					response.Results = _unlockablesRepository.Create(request.Name, request.Type, request.Price, request.Value, request.Display);
				else
					response.AddError(Error.Unlockables.UnlockableAlreadyExists, "Unable to create a new unlockable");
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable get create unlockable", this.GetType()));
			}

			return response;
		}

		public IServicesResponse Unlock(User user, Guid unlockableReference)
		{
			var response = new IServicesResponse(false);
			try
			{
				var unlock = _unlockablesRepository.GetByReference(unlockableReference);

				if (user.CanPurchase(unlock.Price))
				{
					var unlockedAlready = _inMemoryUserRepository.GetUnlockables(user.UserReference, unlockableReference);
					if (!unlock.IsEmpty() && unlockedAlready.IsEmpty())
					{
						_inMemoryUserRepository.AddUnlockable(user.UserReference, unlockableReference);
						_inMemoryUserRepository.UpdateUserBalance(user.UserReference, 0.0M - unlock.Price);

						if (!unlock.IsFree())
						{
							_inMemoryUserRepository.AddTransaction(user.UserReference, UserTransactionType.MarketUnlock, 0.0M - unlock.Price);
							_appConfigRepository.UpdateBalance(unlock.Price);
						}

						response.Results = true;
					}
					else
						response.AddError(Error.Unlockables.UnlockableDoesntExists, "Unable to find unlock");
				}
				else
					response.AddError(Error.Users.UserBalanceBelowRequirements, "Unable to unlock item.");
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to unlock", this.GetType()));
			}

			return response;
		}

		public IServicesResponse Delete(Guid reference)
		{
			var item = _unlockablesRepository.GetByReference(reference);
			if (!item.IsEmpty())
			{
				_inMemoryUserRepository.DeleteAllUnlockables(reference);
				_unlockablesRepository.Delete(item);

				if (item.Type == UnlockableTypes.AvatarUrl)
				{
					var fileUrl = $"{FOLDER_PATHS["UserSvgs"]}{item.Value}";
					if (File.Exists(fileUrl))
						File.Delete(fileUrl);
				}

				return new IServicesResponse(true);
			}
			else
				return new IServicesResponse(false);
		}

		public IServicesResponse Gift(User user, GiftUnlockableRequest request)
		{
			var response = new IServicesResponse(false);
			try
			{
				var giftee = _inMemoryUserRepository.GetUserByUsername(request.GifteeReference);
				if (!giftee.IsValidUser())
				{
					response.AddError(Error.Users.NoUsersFound, $"Unable to find user with name '{request.GifteeReference}'");
					return response;
				}

				var unlock = _unlockablesRepository.GetByReference(request.UnlockableReference);

				if (user.CanPurchase(unlock.Price))
				{
					var unlockedAlready = _inMemoryUserRepository.GetUnlockables(giftee.UserReference, request.UnlockableReference);
					if (!unlock.IsEmpty() && unlockedAlready.IsEmpty())
					{
						_inMemoryUserRepository.AddUnlockable(giftee.UserReference, request.UnlockableReference);
						_inMemoryUserRepository.UpdateUserBalance(user.UserReference, 0.0M - unlock.Price);
						_inMemoryUserRepository.AddTransaction(user.UserReference, UserTransactionType.MarketGift, 0.0M - unlock.Price);
						_inMemoryUserRepository.AddGiftGivenStat(user.UserReference);
						_appConfigRepository.UpdateBalance(unlock.Price);
						response.Results = true;
					}
					else
						response.AddError(Error.Unlockables.UnlockableDoesntExists, "Unable to find unlock");
				}
				else
					response.AddError(Error.Users.UserBalanceBelowRequirements, "Unable to gift item");


			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable gift unlockable", this.GetType()));
			}

			return response;
		}

		public IServicesResponse Update(UpdateUnlockableRequest request)
		{
			var response = new IServicesResponse(new UnlockableResponse());
			try
			{
				var exists = _unlockablesRepository.GetByReference(request.Reference);
				if (!exists.IsEmpty())
				{
					var item = _unlockablesRepository.Update(request.Reference, request.Name, request.Type, request.Price, request.Value, request.Display);
					if (!item.IsEmpty())
						response.Results = item;
					else
						response.AddError(Error.Unlockables.UnlockableDidntUpdate, "Unable to update due to an unknown error");
				}
				else
					response.AddError(Error.Unlockables.UnlockableDidntUpdate, "Unable to find unlockable with information given");
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable get update unlockable", this.GetType()));
			}

			return response;
		}

		public IServicesResponse Populate()
		{
			var response = new IServicesResponse(false);
			try
			{
				var unlocksToCreate = new List<Unlockable>
				{
					{new Unlockable("Theme","Dark Mode", 25, "darkmode", true)},
					{new Unlockable("Theme","Default Theme", 0, "default", true)},
					{new Unlockable("Theme","Neon",  50, "neon", true)},
					{new Unlockable("Theme","Cool Mint", 10, "cool-mint", true)},
					{new Unlockable("Theme","Pretty In Pastel", 15, "pretty-in-pastel", true)},

					{new Unlockable("Avatar","Default Avatar", 0, "basic-avatar.svg", true)},
					{new Unlockable("Avatar","Biker Dude", 5, "biker-dude.svg", true)},
					{new Unlockable("Avatar","Black Panther", 50, "superhero-black-panther.svg", true)},
					{new Unlockable("Avatar","Cuppa Joe", 10, "coffee-cup.svg", true)},
					{new Unlockable("Avatar","Sad Cloud", 15, "crying-cloud.svg", true)},
					{new Unlockable("Avatar","Harley Quinn", 25, "harley-quinn.svg", true)},
					{new Unlockable("Avatar","Poly Wolf", 0, "poly-wolf.svg", false)},

					{new Unlockable("Title","Mr", 3, "Mr", true)},
					{new Unlockable("Title","Mrs", 3, "Mrs", true)},
					{new Unlockable("Title","King", 25, "King", true)},
					{new Unlockable("Title","Queen", 25, "Queen", true)},
					{new Unlockable("Title","👑", 50, "👑", true)},
					{new Unlockable("Title","😍", 50, "😍", true)},

					{new Unlockable("Particle","Firework", 20, "Firework", true)},
					{new Unlockable("Particle","Square", 10, "Square", true)},
					{new Unlockable("Particle","Hearts", 20, "Heart Emoji", true)},
					{new Unlockable("Particle","💵 🤑 💲", 0, "TradeProfitParticle", false)},

					{new Unlockable("Font","Default Font", 0, "'Rubik', Sans Serif", true)},
					{new Unlockable("Font","DynaPuff", 20, "'DynaPuff', Sans Serif", true)},
					{new Unlockable("Font","Montserrat", 5, "'Montserrat', Sans Serif", true)},
					{new Unlockable("Font","Dosis", 5, "'Dosis', Sans Serif", true)},
					{new Unlockable("Font","Indie Flower", 35, "'Indie Flower', Cursive", true)},
				};

				foreach (var unlock in unlocksToCreate)
					_unlockablesRepository.Create(unlock.Name, unlock.Type, unlock.Price, unlock.Value, unlock.Display);
			}
			catch (Exception ex)
			{
				response.AddError(ex);
			}

			return response;
		}
	}
}
