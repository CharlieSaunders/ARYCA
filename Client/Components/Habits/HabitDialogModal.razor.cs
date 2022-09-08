using Common.Entities.Habits;
using Microsoft.AspNetCore.Components;

namespace Client.Components.Habits
{
	public partial class HabitDialogModal
	{
		[Parameter]
		public Habit SelectedHabit { get; set; }

		[Parameter]
		public EventCallback<Habit> OnClose { get; set; }

		[Parameter]
		public ModalType DisplayType { get; set; }

		[Parameter]
		public string Theme { get; set; }

		private Task ModalCancel()
		{
			return OnClose.InvokeAsync(null);
		}

		private Task ModalOk()
		{
			return OnClose.InvokeAsync(SelectedHabit);
		}
	}
}
