using Common.Classes;
using Common.Classes.ErrorHandling;
using Common.Classes.Users;
using Common.Data;
using Common.DTO.Logs;
using Common.DTO.Pledges;
using Common.Entities.Logs;
using Common.Entities.Pledges;
using Common.Entities.Users;
using Infrastructure.InMemory;
using Infrastructure.Repositories;

namespace Services.Routes.Pledges
{
	public interface IPledgesService
	{
		public IServicesResponse GetAll();
		public IServicesResponse Create(CreatePledgeRequest request);
		public IServicesResponse Update(UpdatePledgeRequest request);
		public IServicesResponse Delete(Guid pledgeReference);

		public IServicesResponse Assign(User user, AssignPledgeRequest request);
		public IServicesResponse Assigned(User user);
		public IServicesResponse UpdateAssignedStatus(User user, UpdatePledgeStatusRequest request);
	}

	public class PledgesService : IPledgesService
	{
		private readonly InMemoryUserRepositories _inMemoryUserRepository;
		private readonly IPledgesRepository _pledgesRepository;
		private readonly ILogsService _logsService;

		public PledgesService(DataContext db)
		{
			_pledgesRepository = new PledgesRepository(db);
			_logsService = new LogsService(db);
			_inMemoryUserRepository = new InMemoryUserRepositories(db);
		}

		public IServicesResponse Create(CreatePledgeRequest request)
		{
			var response = new IServicesResponse(new Pledge());
			try
			{
				response.Results = _pledgesRepository.Create(request.Name, request.Details);
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to create pledge", this.GetType()));
			}
			return response;
		}

		public IServicesResponse Delete(Guid pledgeReference)
		{
			var response = new IServicesResponse(false);
			try
			{
				response.Results = _pledgesRepository.Delete(pledgeReference);
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to delete plegde", this.GetType()));
			}

			return response;
		}

		public IServicesResponse GetAll()
		{
			var response = new IServicesResponse(new List<Pledge>());
			try
			{
				response.Results = _pledgesRepository.GetAll();
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to get all pledges", this.GetType()));
			}

			return response;
		}

		public IServicesResponse Update(UpdatePledgeRequest request)
		{
			var response = new IServicesResponse(new Pledge());
			try
			{
				response.Results = _pledgesRepository.Update(request.PledgeReference, request.Name, request.Details);
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to update pledge", this.GetType()));
			}

			return response;
		}

		public IServicesResponse Assign(User user, AssignPledgeRequest request)
		{
			var response = new IServicesResponse(new UserPledges());
			try
			{
				var pledgeExists = _pledgesRepository.GetByReference(request.PledgeReference);
				var otherUser = _inMemoryUserRepository.GetUserByUsername(request.AssigneeUsername);

				if (!pledgeExists.IsEmpty() && otherUser.UserReference != Guid.Empty)
					response.Results = _inMemoryUserRepository.AddPledge(new RepositoryAddPledgeRequest(user, otherUser, request));
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to assign pledge", this.GetType()));
			}

			return response;
		}

		public IServicesResponse Assigned(User user)
		{
			var response = new IServicesResponse(new List<AssignedPledgeResponse>());
			try
			{
				var allPledges = _pledgesRepository.GetAll();
				var allAssigned = _inMemoryUserRepository.GetAllPledges(user.UserReference);

				foreach (var assigned in allAssigned)
				{
					var matchingPledge = allPledges.FirstOrDefault(x => x.Reference == assigned.PledgeReference);
					if (matchingPledge is not null)
					{
						var newAssignedPledge = new AssignedPledgeResponse
						{
							PledgeId = assigned.Id,
							Name = matchingPledge.Name,
							Details = assigned.AdditionalInformation,
							Value = assigned.Value,
						};

						newAssignedPledge.Assigner = assigned.AssignerReference == user.UserReference
												? new SlimUser(user)
												: new SlimUser(_inMemoryUserRepository.GetUserByReference(assigned.AssignerReference));

						newAssignedPledge.Assignee = assigned.AssigneeReference == user.UserReference
												? new SlimUser(user)
												: new SlimUser(_inMemoryUserRepository.GetUserByReference(assigned.AssigneeReference));

						newAssignedPledge.SetStatus(assigned.AssigneeAccepted, assigned.AssigneeCompleted, assigned.AssignerSignedOff);
						newAssignedPledge.MarkCurrentOwner(user.UserReference);

						response.Results.Add(newAssignedPledge);
					}
				}
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to get assigned pledge", this.GetType()));
			}

			return response;
		}

		public IServicesResponse UpdateAssignedStatus(User user, UpdatePledgeStatusRequest request)
		{
			var response = new IServicesResponse(false);
			try
			{
				var assignedPledge = _inMemoryUserRepository.GetPledge(request.PledgeId);
				if (!assignedPledge.IsEmpty())
				{
					assignedPledge = PledgeStatusUpdater.From(assignedPledge, request.NewStatus);
					var shouldUpdate = PledgeStatusUpdater.ShouldUpdate(request.NewStatus);
					if (assignedPledge.AssignerSignedOff || shouldUpdate)
					{
						_inMemoryUserRepository.UpdatePledge(assignedPledge);
						response.Results = true;
					}
					else
					{
						_inMemoryUserRepository.DeletePledge(assignedPledge.Id);
						var shouldUpdateBalance = request.NewStatus == PledgeStatuses.PledgeStatus.SignedOff;
						if (shouldUpdateBalance)
						{
							_inMemoryUserRepository.UpdateUserBalance(assignedPledge.AssigneeReference, assignedPledge.Value);
							_inMemoryUserRepository.AddTransaction(assignedPledge.AssigneeReference, UserTransactionType.PledgeComplete, assignedPledge.Value);

							_inMemoryUserRepository.UpdateUserBalance(assignedPledge.AssignerReference, 0.0M - assignedPledge.Value);
							_inMemoryUserRepository.AddTransaction(assignedPledge.AssignerReference, UserTransactionType.PledgeComplete, 0.0M - assignedPledge.Value);

							_inMemoryUserRepository.AddPledgeCompleteStat(assignedPledge.AssigneeReference);
						}

						response.Results = true;
					}

				}
				else
				{
					response.AddError(new Error(Error.Pledges.UnableToFindPledge, $"Unable to find assigned pledge with id '{request.PledgeId}'", 400, string.Empty));
				}
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to update assigned pledge", this.GetType()));
			}

			return response;
		}
	}
}
