using ARYCA_Tests.Helpers;
using Common.Classes;
using Common.Data;
using Common.DTO.Pledges;
using Common.Entities.Pledges;
using Common.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Services.Controllers;

namespace ARYCA_Tests.Services.Routes.Pledges
{
	[TestFixture]
	public sealed class GivenARequestForTheAssigneeToAccept
	{
		private IServicesResponse _apiResponse;
		private PledgesController _pledgesController;
		private DataContext _datacontext;

		private User _user, _user2;
		private UpdatePledgeStatusRequest _request;
		private Pledge _pledge;
		private UserPledges _userPledge;
		private UserPledges _updatedPledgeSubject;

		[OneTimeSetUp]
		public void Setup()
		{
			_user = UserHelper.GetActiveUser();
			_user2 = UserHelper.GetInactiveUser("Two", 1);
			_pledge = PledgeHelper.GetOldPledge();

			_userPledge = PledgeHelper.GetUserPledge(_user.UserReference, _user2.UserReference, _pledge.Reference, _pledge.Details);

			using (_datacontext = DatabaseHelper.GetDatabase())
			{
				_datacontext = DatabaseHelper.SeedUsers(_datacontext, new List<User>() { _user, _user2 });
				_datacontext = DatabaseHelper.SeedPledges(_datacontext, new List<Pledge>() { _pledge });
				_datacontext = DatabaseHelper.SeedAssignedPledges(_datacontext, new List<UserPledges>() { _userPledge });

				_request = new UpdatePledgeStatusRequest
				{
					PledgeId = _datacontext.UserPledges.First(x => x.PledgeReference == _userPledge.PledgeReference).Id,
					NewStatus = PledgeStatuses.PledgeStatus.AwaitingCompletion
				};

				_pledgesController = ControllerHelper.GetPledgesController(_datacontext, HttpContextHelper.GetWithArycaUserReference(_user.UserReference.ToString()));

				var subject = _pledgesController.UpdateAssignedStatus(_request).Result as ObjectResult;
				_apiResponse = (IServicesResponse)subject.Value;

				_updatedPledgeSubject = _datacontext.UserPledges.First(x => x.Id == _request.PledgeId);
			}
		}

		[Test]
		public void ThenTheResponseDoesNotHaveAnError()
		{
			Assert.That(_apiResponse.HasError, Is.False);
		}

		[Test]
		public void ThenTheAssignedPledgeStatusWasUpdated()
		{
			Assert.That(_updatedPledgeSubject.AssigneeAccepted, Is.True);
			Assert.That(_updatedPledgeSubject.AssigneeCompleted, Is.False);
			Assert.That(_updatedPledgeSubject.AssignerSignedOff, Is.False);
		}
	}
}
