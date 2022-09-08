using Common.Entities.Habits;
using Microsoft.AspNetCore.Components;

namespace Client.Components.Habits
{
	public partial class HabitsDialogModal
	{
		[Parameter]
		public List<Habit> SelectedHabits { get; set; }

		[Parameter]
		public EventCallback<Habit> OnClose { get; set; }

		[Parameter]
		public ModalType DisplayType { get; set; }

		[Parameter]
		public string Theme { get; set; }

		private Habit returnedHabit { get; set; }

		private Task ModalCancel()
		{
			return OnClose.InvokeAsync(null);
		}

		private Task AssignHabit(Habit habit)
		{
			return OnClose.InvokeAsync(habit);
		}
	}
}
