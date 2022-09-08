using Client.Helpers;
using Client.ServicesBridge;
using Common.DTO.Unlockables;
using Common.Entities.Unlockables;

namespace Client.Pages
{
	public partial class Marketplace
	{
		private UsersBridge _usersBridge;
		private UnlockablesBridge _unlockablesBridge;
		private List<UnlockableResponse> _lockedThemes;
		private List<UnlockableResponse> _lockedAvatars;
		private List<UnlockableResponse> _lockedTitles;
		private List<UnlockableResponse> _lockedParticles;
		private List<UnlockableResponse> _lockedFonts;
		private bool _demoModalOpened = false;
		private UnlockableResponse _demoTheme;
		private string _userTheme;

		private bool _giftingModal = false;
		private UnlockableResponse _selectedGift;

		private bool _themesLoaded = false;
		private bool _avatarsLoaded = false;
		private bool _titlesLoaded = false;
		private bool _particlesLoaded = false;
		private bool _fontsLoaded = false;

		protected override async Task OnInitializedAsync()
		{
			await _localStorage.SetItemAsync("LastPage", "Marketplace");
			_usersBridge = new UsersBridge(_genericHttpClient, _toasterService);

			_unlockablesBridge = new UnlockablesBridge(_genericHttpClient, _toasterService);

			_userTheme = _userState.GetUserTheme();
			await _userState.UpdateUser(await LocalStorageHelper.GetAuthToken(_localStorage));

			await GetLockedItems();
			await GetCurrentUser();
		}

		private void OpenDialogModal(UnlockableResponse theme)
		{
			_demoTheme = theme;
			_demoModalOpened = true;
			_giftingModal = false;
			StateHasChanged();
		}

		private void OpenGiftDialogModal(UnlockableResponse item)
		{
			_selectedGift = item;
			_userTheme = _userState.GetUserTheme();
			_demoModalOpened = false;
			_giftingModal = true;
			StateHasChanged();
		}

		private void OnDialogClose(string demo)
		{
			_demoModalOpened = false;
			_giftingModal = false;
			StateHasChanged();
		}

		private async void OnGiftDialogClose(GiftUnlockableRequest request)
		{
			_demoModalOpened = false;
			_giftingModal = false;

			if (request is not null)
				await GiftUnlockable(request);

			StateHasChanged();
		}

		protected async Task GetLockedItems()
		{
			var allItems = await _unlockablesBridge.GetAll(await LocalStorageHelper.GetAuthToken(_localStorage));
			var lockedItems = allItems.Where(x => !x.Unlocked && x.Display).ToList();

			_lockedAvatars = lockedItems.Where(x => x.Type == UnlockableTypes.AvatarUrl).OrderByDescending(x => x.Price).ToList();
			_lockedThemes = lockedItems.Where(x => x.Type == UnlockableTypes.Theme).OrderByDescending(x => x.Price).ToList();
			_lockedTitles = lockedItems.Where(x => x.Type == UnlockableTypes.Title).OrderByDescending(x => x.Price).ToList();
			_lockedParticles = lockedItems.Where(x => x.Type == UnlockableTypes.Particle).OrderByDescending(x => x.Price).ToList();
			_lockedFonts = lockedItems.Where(x => x.Type == UnlockableTypes.Font).OrderByDescending(x => x.Price).ToList();
			_themesLoaded = true;
			_fontsLoaded = true;
			_avatarsLoaded = true;
			_titlesLoaded = true;
			_particlesLoaded = true;

			StateHasChanged();
		}

		protected async Task GetCurrentUser()
		{
			var jwt = await LocalStorageHelper.GetAuthToken(_localStorage);
			await _userState.UpdateUser(jwt);
			var userReference = _userState.GetUserReference();

			if (userReference != Guid.Empty)
				_userState.SetUser(await _usersBridge.GetUserByReference(userReference, jwt));

			StateHasChanged();
		}

		private async Task Unlock(UnlockableResponse item)
		{
			if (item.Price <= _userState.GetUserBalance())
				await _unlockablesBridge.Unlock(item.UnlockableReference, item.Name, await LocalStorageHelper.GetAuthToken(_localStorage));

			await GetCurrentUser();
			await GetLockedItems();
		}

		private async Task GiftUnlockable(GiftUnlockableRequest request)
		{
			if (_selectedGift.Price < _userState.GetUserBalance())
				await _unlockablesBridge.Gift(request, await LocalStorageHelper.GetAuthToken(_localStorage));

			await GetCurrentUser();
			await GetLockedItems();
		}

		private static string GetPrice(decimal price)
		{
			if (price == 0)
			{
				return "FREE";
			}
			else
			{
				if (price % 1 == 0)
				{
					return string.Format("{0:0.##}", price);
				}
				else
				{
					return $"{price}";
				}

			}
		}
	}
}
