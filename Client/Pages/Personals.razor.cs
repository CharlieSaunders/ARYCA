using Client.Components;
using Client.Helpers;
using Client.ServicesBridge;
using Client.States.Toast.Types;
using Common.DTO.Habits;
using Common.DTO.Pledges;
using Common.Entities.Habits;
using Common.Entities.Pledges;

namespace Client.Pages
{
	public partial class Personals
	{
		private int ITEMS_PER_PAGE = 10;
		private ModalType _modalType;
		private string _userTheme;

		protected override async Task OnInitializedAsync()
		{
			await _localStorage.SetItemAsync("LastPage", "Personals");
			_userTheme = _userState.GetUserTheme();

			_habitsBridge = new HabitsBridge(_genericHttpClient, _toasterService);
			_pledgesBridge = new PledgesBridge(_genericHttpClient, _toasterService);

			await _userState.UpdateUser(await LocalStorageHelper.GetAuthToken(_localStorage));

			await GetAssignedHabits();
			await GetHabits();
			await GetPledges();
		}

		private void CloseAllModals()
		{
			_singleHabitDialogModalOpen = false;
			_multipleHabitDialogModalOpen = false;
			_singlePledgeDialogModalOpen = false;
			_assignPledgeDialogModalOpen = false;
			_viewAssignedPledgeModalOpen = false;
		}

		#region Pledges

		private Pledge _selectedPledge;
		private AssignPledgeRequest _assignPledgeRequest;
		private AssignedPledgeResponse _assignedPledgeResponse;

		private bool _singlePledgeDialogModalOpen;
		private bool _assignPledgeDialogModalOpen;
		private bool _viewAssignedPledgeModalOpen;

		private bool _assignedPledges = false;
		private bool _allPledgesLoaded = false;
		private bool _allAssignedPledgesLoaded = false;

		private PledgesBridge _pledgesBridge;
		private List<Pledge> _allPledges;
		private List<AssignedPledgeResponse> _assignedToUserPledges;
		private List<AssignedPledgeResponse> _allAssigned;

		protected async Task GetPledges()
		{
			var jwt = await LocalStorageHelper.GetAuthToken(_localStorage);
			_allPledges = await _pledgesBridge.GetAll(jwt);
			_allAssigned = await _pledgesBridge.GetAssigned(jwt);
			_assignedToUserPledges = _allAssigned.Where(x => x.IsWithYou).ToList();

			_assignedPledges = true;
			_allPledgesLoaded = true;
			_allAssignedPledgesLoaded = true;

			StateHasChanged();
		}

		private async void OnDialogClose(Pledge pledge)
		{
			CloseAllModals();

			if (pledge != null)
			{
				switch (_modalType)
				{
					case ModalType.Create: await Create(pledge); break;
					case ModalType.Update: await Update(pledge); break;
				}
			}
			StateHasChanged();
		}

		private async void OnDialogClose(AssignPledgeRequest request)
		{
			CloseAllModals();
			_assignPledgeDialogModalOpen = false;
			if (request != null)
			{
				if (request.Value <= _userState.GetUserBalance())
					await Assign(request);
				else
					_toasterService.AddToast(SimpleToast.NewToast("Assign Pledge", "Unable to assign pledge due to missing balance", MessageColour.Danger, 5));
			}
			StateHasChanged();
		}

		private async void OnDialogClose(UpdatePledgeStatusRequest updatePledgeRequest)
		{
			CloseAllModals();
			if (updatePledgeRequest != null)
			{
				await UpdateAssignedPledge(updatePledgeRequest);
			}
			StateHasChanged();
		}

		private void OpenDialogModal(Pledge pledge, ModalType modalType)
		{
			_selectedPledge = pledge;
			_modalType = modalType;
			_singlePledgeDialogModalOpen = true;
			_assignPledgeDialogModalOpen = false;
			_viewAssignedPledgeModalOpen = false;
			StateHasChanged();
		}

		private void OpenAssignDialogModal(AssignPledgeRequest request, Pledge pledge, ModalType modalType)
		{
			_selectedPledge = pledge;
			_assignPledgeRequest = request;
			_modalType = modalType;
			_singlePledgeDialogModalOpen = false;
			_assignPledgeDialogModalOpen = true;
			_viewAssignedPledgeModalOpen = false;
			StateHasChanged();
		}

		private void OpenAssignedPledgeModal(AssignedPledgeResponse pledgeResponse, ModalType modalType)
		{
			_assignedPledgeResponse = pledgeResponse;
			_modalType = modalType;
			_viewAssignedPledgeModalOpen = true;
			_singlePledgeDialogModalOpen = false;
			_assignPledgeDialogModalOpen = false;
			StateHasChanged();
		}

		protected async Task Create(Pledge pledge)
		{
			var createPledgeRequest = new CreatePledgeRequest
			{
				Name = pledge.Name,
				Details = pledge.Details
			};

			await _pledgesBridge.Create(createPledgeRequest, await LocalStorageHelper.GetAuthToken(_localStorage));
			await GetPledges();
		}

		protected async Task Update(Pledge pledge)
		{
			var updatePledgeRequest = new UpdatePledgeRequest
			{
				PledgeReference = pledge.Reference,
				Name = pledge.Name,
				Details = pledge.Details
			};

			await _pledgesBridge.Update(updatePledgeRequest, await LocalStorageHelper.GetAuthToken(_localStorage));
			await GetPledges();
		}

		protected async Task Assign(AssignPledgeRequest request)
		{
			await _pledgesBridge.Assign(request, await LocalStorageHelper.GetAuthToken(_localStorage));
			await GetPledges();
		}

		protected async Task UpdateAssignedPledge(UpdatePledgeStatusRequest request)
		{
			await _pledgesBridge.UpdateAssignedStatus(request, await LocalStorageHelper.GetAuthToken(_localStorage));
			await GetPledges();
		}

		protected async Task DeletePledge(Pledge pledge)
		{
			await _pledgesBridge.DeletePlegde(pledge.Reference, await LocalStorageHelper.GetAuthToken(_localStorage));
			await GetPledges();
		}

		#endregion

		#region Habits

		private List<Habit> _allHabits;
		private Habit _selectedHabit;
		private List<Habit> _selectedHabits;
		private bool _singleHabitDialogModalOpen = false;
		private bool _multipleHabitDialogModalOpen = false;
		private HabitsBridge _habitsBridge;
		private List<Habit> _assignedHabits;
		private bool _assignedHabitsLoaded = false;
		private List<Habit> _assignedHabitsPaginated;

		private int HABIT_PAGE = 0;
		private int HABIT_MAX_PAGE;

		private async void OnDialogClose(Habit habit)
		{
			CloseAllModals();
			if (habit != null)
			{
				switch (_modalType)
				{
					case ModalType.Create: await Create(habit); break;
					case ModalType.Select: await Assign(habit); break;
					case ModalType.Update: await Update(habit); break;
				}
			}
			StateHasChanged();
		}

		private void ChangeHabitPage(int value)
		{
			var newPageValue = HABIT_PAGE += value;
			HABIT_PAGE = newPageValue < 0 ? 0 : newPageValue;
			var assignedHabits = _assignedHabits.Skip(HABIT_PAGE * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToList();
			_assignedHabitsPaginated = assignedHabits.Count == 0 ? _assignedHabitsPaginated : assignedHabits;
			StateHasChanged();
		}

		private void OpenDialogModal(Habit habit, ModalType modalType)
		{
			_selectedHabit = habit;
			_modalType = modalType;
			_singleHabitDialogModalOpen = true;
			StateHasChanged();
		}

		private void OpenDialogModal(List<Habit> habits, ModalType modalType)
		{
			foreach (var assignedHabit in _assignedHabits)
				habits.Remove(habits.First(x => x.Reference == assignedHabit.Reference));

			_selectedHabits = habits;
			_modalType = modalType;
			_multipleHabitDialogModalOpen = true;
			StateHasChanged();
		}


		protected async Task GetHabits()
		{
			_allHabits = await _habitsBridge.GetAllHabits(await LocalStorageHelper.GetAuthToken(_localStorage));
			StateHasChanged();
		}

		protected async Task GetAssignedHabits()
		{
			_assignedHabits = await _habitsBridge.GetAllHabitsAssigned(await LocalStorageHelper.GetAuthToken(_localStorage));
			_assignedHabitsPaginated = _assignedHabits.Skip(HABIT_PAGE * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToList();
			_assignedHabitsLoaded = true;
			HABIT_MAX_PAGE = (_assignedHabits.Count - 1) / ITEMS_PER_PAGE;
			StateHasChanged();
		}

		protected async Task Create(Habit habit)
		{
			var createHabitRequest = new CreateHabitRequest
			{
				Name = habit.Name,
				Description = habit.Description
			};

			await _habitsBridge.AddHabit(createHabitRequest, habit.Name, await LocalStorageHelper.GetAuthToken(_localStorage));
			await GetHabits();
		}

		protected async Task Complete(Habit habit)
		{
			await _habitsBridge.Complete(habit.Reference, habit.Name, await LocalStorageHelper.GetAuthToken(_localStorage));
			await GetAssignedHabits();
		}

		protected async Task Unassign(Habit habit)
		{
			await _habitsBridge.Unassign(habit.Reference, habit.Name, await LocalStorageHelper.GetAuthToken(_localStorage));
			await GetAssignedHabits();
		}

		protected async Task Assign(Habit habit)
		{
			await _habitsBridge.Assign(habit.Reference, habit.Name, await LocalStorageHelper.GetAuthToken(_localStorage));
			await GetAssignedHabits();
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
			await GetAssignedHabits();
			await GetHabits();
		}

		#endregion
	}
}
