using Client.Components;
using Client.Helpers;
using Client.ServicesBridge;
using Client.States.Toast.Types;
using Common.Classes.Users;
using Common.DTO.Configs;
using Common.DTO.Habits;
using Common.DTO.Unlockables;
using Common.Entities.Configuration;
using Common.Entities.Habits;

namespace Client.Pages
{
	public partial class Admin
	{
		private string _userTheme;
		private int ITEMS_PER_PAGE = 14;

		private UnlockablesBridge _unlockablesBridge;
		private UsersBridge _usersBridge;
		private HabitsBridge _habitsBridge;
		private ConfigManagerBridge _configsBridge;

		private List<UnlockableResponse> _marketPlaceItems;
		private List<UnlockableResponse> _marketplaceItemsPaginated;
		private UnlockableResponse _selectedItem;
		private bool _editMarketplaceDialogModal = false;
		private int MARKET_PAGE = 0;
		private int MARKET_MAX_PAGE;

		private List<Habit> _allHabits;
		private List<Habit> _allHabitsPaginated;
		private Habit _selectedHabit;
		private bool _editHabitDialogModal = false;
		private int HABITS_PAGE = 0;
		private int HABITS_MAX_PAGE;

		private ApplicationConfiguration _appConfig;
		private bool _editBackgroundImageDialogModal = false;

		private List<SlimUser> _users;

		private ModalType _modalType;

		protected override async Task OnInitializedAsync()
		{
			await _localStorage.SetItemAsync("LastPage", "Admin");

			_unlockablesBridge = new UnlockablesBridge(_genericHttpClient, _toasterService);
			_usersBridge = new UsersBridge(_genericHttpClient, _toasterService);
			_habitsBridge = new HabitsBridge(_genericHttpClient, _toasterService);
			_configsBridge = new ConfigManagerBridge(_genericHttpClient, _toasterService);

			_userTheme = _userState.GetUserTheme();
			await _userState.UpdateUser(await LocalStorageHelper.GetAuthToken(_localStorage));

			await GetAllHabits();
			await GetAllMarketPlaceItems();
			await GetSlimUsers();
			await GetConfigs();
		}

		private void CloseAllDialogModals()
		{
			_editHabitDialogModal = false;
			_editMarketplaceDialogModal = false;
			_editBackgroundImageDialogModal = false;
		}

		// Market region
		#region Market
		public async Task GetAllMarketPlaceItems()
		{
			_marketPlaceItems = await _unlockablesBridge.GetAll(await LocalStorageHelper.GetAuthToken(_localStorage));
			_marketplaceItemsPaginated = _marketPlaceItems.Skip(MARKET_PAGE * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToList();
			MARKET_MAX_PAGE = (_marketPlaceItems.Count - 1) / ITEMS_PER_PAGE;
			StateHasChanged();
		}

		public void ChangeMarketPlacePage(int change)
		{
			var newPageValue = MARKET_PAGE += change;
			MARKET_PAGE = newPageValue < 0 ? 0 : newPageValue;
			var marketItems = _marketPlaceItems.Skip(MARKET_PAGE * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToList();
			_marketplaceItemsPaginated = marketItems.Count == 0 ? _marketplaceItemsPaginated : marketItems;
			StateHasChanged();
		}

		private void OpenMarketPlaceModal(UnlockableResponse item, ModalType modalType)
		{
			_selectedItem = item;
			_modalType = modalType;
			_editHabitDialogModal = false;
			_editMarketplaceDialogModal = true;
			_editBackgroundImageDialogModal = false;
			StateHasChanged();
		}

		private async void OnMarketplaceDialogClose(UnlockableResponse item)
		{
			CloseAllDialogModals();

			if (item != null)
				await UpdateItem(item);

			StateHasChanged();
		}

		protected async Task UpdateItem(UnlockableResponse item)
		{
			var updateItemRequest = new UpdateUnlockableRequest
			{
				Reference = item.UnlockableReference,
				Name = item.Name,
				Value = item.Value,
				Price = item.Price,
				Type = item.Type
			};

			await _unlockablesBridge.Update(updateItemRequest, await LocalStorageHelper.GetAuthToken(_localStorage));
			await GetAllMarketPlaceItems();
		}

		protected async Task DeleteMarketplaceItem(UnlockableResponse item)
		{
			await _unlockablesBridge.Delete(item.UnlockableReference, await LocalStorageHelper.GetAuthToken(_localStorage));
			await GetAllMarketPlaceItems();
		}

		#endregion

		// Users Region
		#region Users
		protected async Task GetSlimUsers()
		{
			_users = await _usersBridge.GetSlimUsers(await LocalStorageHelper.GetAuthToken(_localStorage));
			StateHasChanged();
		}

		protected void DeleteUser(SlimUser user)
		{
			_toasterService.AddToast(SimpleToast.NewToast($"Delete {user.FirstName}", "This service is not enabled to protect user accounts", MessageColour.Danger, 5));
		}

		#endregion

		// Habits Region
		#region Habits

		protected async Task GetAllHabits()
		{
			_allHabits = await _habitsBridge.GetAllHabits(await LocalStorageHelper.GetAuthToken(_localStorage));
			_allHabitsPaginated = _allHabits.Skip(HABITS_PAGE * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToList();
			HABITS_MAX_PAGE = (_allHabits.Count - 1) / ITEMS_PER_PAGE;
			StateHasChanged();
		}


		public void ChangeHabitPage(int change)
		{
			var newPageValue = HABITS_PAGE += change;
			HABITS_PAGE = newPageValue < 0 ? 0 : newPageValue;
			var habits = _allHabits.Skip(MARKET_PAGE * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToList();
			_allHabitsPaginated = habits.Count == 0 ? _allHabitsPaginated : habits;
			StateHasChanged();
		}

		private void OpenHabitModal(Habit habit, ModalType modalType)
		{
			_selectedHabit = habit;
			_modalType = modalType;
			_editHabitDialogModal = true;
			_editMarketplaceDialogModal = false;
			_editBackgroundImageDialogModal = false;
			StateHasChanged();
		}

		private async void OnHabitDialogClose(Habit habit)
		{
			CloseAllDialogModals();

			if (habit != null)
				await Update(habit);

			StateHasChanged();
		}

		protected async Task Update(Habit habit)
		{
			var updateHabitRequest = new UpdateHabitRequest
			{
				HabitReference = habit.Reference,
				Name = habit.Name,
				Description = habit.Description
			};

			await _habitsBridge.Update(updateHabitRequest, habit.Name, await LocalStorageHelper.GetAuthToken(_localStorage));
			await GetAllHabits();
		}

		protected async Task DeleteHabit(Habit habit)
		{
			await _habitsBridge.DeleteHabit(habit.Reference, await LocalStorageHelper.GetAuthToken(_localStorage));
			await GetAllHabits();
		}
		#endregion

		// Configs Region
		#region Configs

		protected async Task GetConfigs()
		{
			_appConfig = await _localStorage.GetItemAsync<ApplicationConfiguration>("ApplicationConfig");
			StateHasChanged();
		}

		private void OnBackgroundImageDialogClose(string imageUrl)
		{
			CloseAllDialogModals();
			if (imageUrl is not null && imageUrl != _appConfig.LoginImageUrl)
			{
				_appConfig.LoginImageUrl = imageUrl;
			}
			StateHasChanged();
		}

		protected async Task UpdateAppConfig()
		{
			var request = new UpdateAppConfigRequest
			{
				AppName = _appConfig.ApplicationName,
				Date = _appConfig.MemorableDate,
				ImageText = _appConfig.ImageText,
				ImageUrl = _appConfig.LoginImageUrl
			};

			await _configsBridge.UpdateAppConfig(request, await LocalStorageHelper.GetAuthToken(_localStorage));
		}

		private void OpenBackgroundImageDialogModal()
		{
			_editBackgroundImageDialogModal = true;
			StateHasChanged();
		}

		#endregion
	}
}
