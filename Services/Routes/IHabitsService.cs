using Common.Classes;
using Common.Classes.ErrorHandling;
using Common.Data;
using Common.DTO.Habits;
using Common.DTO.Logs;
using Common.Entities.Habits;
using Common.Entities.Logs;
using Common.Entities.Users;
using Infrastructure.EF;
using Infrastructure.EF.Habits;
using Infrastructure.Interfaces;

namespace Services.Routes
{
	public interface IHabitsService
	{
		IServicesResponse GetAll();
		IServicesResponse CreateHabit(CreateHabitRequest request);
		IServicesResponse DeleteHabit(Guid reference);
		IServicesResponse AssignHabit(Guid userReference, Guid habitReference);
		IServicesResponse UnAssignHabit(Guid userReference, Guid habitReference);
		IServicesResponse GetAllAssigned(Guid userReference);
		IServicesResponse CompleteHabit(Guid userReference, Guid habitReference);
		IServicesResponse UpdateHabit(UpdateHabitRequest request);
	}

	public class HabitsService : IHabitsService
	{
		private readonly EFUsersRepositories _inMemoryUserRepository;
		private readonly IHabitsRepository _habitsRepository;
		private readonly ILogsService _logsService;
		private const int HABIT_VALUE = 1;

		public HabitsService(DataContext db)
		{
			_inMemoryUserRepository = new EFUsersRepositories(db);
			_habitsRepository = new EFHabitsRepository(db);
			_logsService = new LogsService(db);
		}

		public IServicesResponse GetAll()
		{
			var response = new IServicesResponse(new List<Habit>());

			try
			{
				response.Results = _habitsRepository.GetAll();
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to get all habits", this.GetType()));
			}

			return response;
		}

		public IServicesResponse GetAllAssigned(Guid userReference)
		{
			var response = new IServicesResponse(new List<Habit>());
			try
			{
				var allHabits = _habitsRepository.GetAll();
				var assignedHabits = _inMemoryUserRepository.GetHabits(userReference);

				foreach (var assignedHabit in assignedHabits)
				{
					var matchingHabit = allHabits.FirstOrDefault(x => x.Reference == assignedHabit.HabitReference);
					if (matchingHabit is not null)
					{
						matchingHabit.Completable = assignedHabit.CanBeCompleted();
						response.Results.Add(matchingHabit);
					}
				}
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to get all assigned habits", this.GetType()));
			}

			return response;
		}

		public IServicesResponse CreateHabit(CreateHabitRequest request)
		{
			var response = new IServicesResponse(new Habit());
			try
			{
				var habitExists = _habitsRepository.Exists(request.Name);
				if (!habitExists)
					response.Results = _habitsRepository.Create(request.Name, request.Description, HABIT_VALUE);
				else
					response.AddError(Error.Habits.UnableToCreateHabit, $"Unable to create habit with name '{request.Name}'");
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to create habit", this.GetType()));
			}

			return response;
		}
		public IServicesResponse DeleteHabit(Guid reference)
		{
			var response = new IServicesResponse(false);
			try
			{
				var habitExists = _habitsRepository.Exists(reference);
				if (habitExists)
				{
					_inMemoryUserRepository.DeleteHabit(reference);
					response.Results = _habitsRepository.Delete(reference);
				}
				else
				{
					response.AddError(Error.Habits.UnableToDeleteHabit, $"Unable to delete habit with reference '{reference}'");
				}
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to delete habit", this.GetType()));
			}

			return response;
		}

		public IServicesResponse AssignHabit(Guid userReference, Guid habitReference)
		{
			var response = new IServicesResponse(false);
			try
			{
				var habitExists = _habitsRepository.Exists(habitReference);
				var habitAssignedAlready = _inMemoryUserRepository.HabitExists(userReference, habitReference);

				if (habitExists && !habitAssignedAlready)
					response.Results = _inMemoryUserRepository.AssignHabit(userReference, habitReference);
				else
					response.AddError(Error.Habits.UnableToDeleteHabit, $"Unable to assign habit with reference '{habitReference}'");
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to assign habit", this.GetType()));
			}

			return response;
		}
		public IServicesResponse UnAssignHabit(Guid userReference, Guid habitReference)
		{
			var response = new IServicesResponse(false);
			try
			{
				var habitExists = _habitsRepository.Exists(habitReference);

				if (habitExists)
					response.Results = _inMemoryUserRepository.UnAssignHabit(userReference, habitReference);
				else
					response.AddError(Error.Habits.UnableToDeleteHabit, $"Unable to assign habit with reference '{habitReference}'");
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to unassign habit", this.GetType()));
			}

			return response;
		}

		public IServicesResponse CompleteHabit(Guid userReference, Guid habitReference)
		{
			var response = new IServicesResponse(false);
			try
			{
				var habitExists = _habitsRepository.Exists(habitReference);
				if (habitExists)
				{
					var habitCompleteValid = _inMemoryUserRepository.CompleteHabit(userReference, habitReference);

					if (habitCompleteValid)
					{
						var habit = _habitsRepository.GetByReference(habitReference);
						_inMemoryUserRepository.UpdateUserBalance(userReference, habit.Value);
						_inMemoryUserRepository.AddTransaction(userReference, UserTransactionType.CompleteHabit, habit.Value);
						_inMemoryUserRepository.AddHabitCompleteStat(userReference);
						response.Results = true;
					}
				}
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to complete habit", this.GetType()));
			}
			return response;
		}

		public IServicesResponse UpdateHabit(UpdateHabitRequest request)
		{
			var response = new IServicesResponse(new Habit());
			try
			{
				var habitExists = _habitsRepository.Exists(request.HabitReference);
				if (habitExists)
					response.Results = _habitsRepository.Update(request.HabitReference, request.Name, request.Description, HABIT_VALUE);

			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to update habit", this.GetType()));
			}
			return response;
		}
	}
}