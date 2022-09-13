using ARYCA_Tests.Helpers;
using ARYCA_Tests.Helpers.TypeHelpers;
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
	public sealed class GivenARequestToAssignAPledge
	{
		private IServicesResponse _apiResponse;
		private PledgesController _pledgesController;
		private DataContext _datacontext;

		private User _user, _user2;
		private AssignPledgeRequest _request;
		private Pledge _pledge;
		private UserPledges _userPledge;
		private List<UserPledges> _allPlegdes;

		[OneTimeSetUp]
		public void Setup()
		{
			_user = UserHelper.GetActiveUser();
			_user2 = UserHelper.GetInactiveUser("Two", 1);
			_pledge = PledgeHelper.GetOldPledge();

			_request = new AssignPledgeRequest
			{
				PledgeReference = _pledge.Reference,
				AssignerReference = _user.UserReference,
				AssigneeUsername = _user2.Username,
				AdditionalInformation = "Test",
				Value = 1
			};

			_userPledge = PledgeHelper.GetUserPledge(_user.UserReference, _user2.UserReference, _pledge.Reference, _pledge.Details);

			using (_datacontext = DatabaseHelper.GetDatabase())
			{
				_datacontext = DatabaseHelper.SeedUsers(_datacontext, new List<User>() { _user, _user2 });
				_datacontext = DatabaseHelper.SeedPledges(_datacontext, new List<Pledge>() { _pledge });
				_datacontext = DatabaseHelper.SeedAssignedPledges(_datacontext, new List<UserPledges>() { _userPledge });

				_pledgesController = ControllerHelper.GetPledgesController(_datacontext, HttpContextHelper.GetWithArycaUserReference(_user.UserReference.ToString()));

				var subject = _pledgesController.Assign(_request).Result as ObjectResult;
				_apiResponse = (IServicesResponse)subject.Value;

				var apiResponseAssignedPledge = (UserPledges)_apiResponse.Results;
				_allPlegdes = _datacontext.UserPledges.Where(x => x.Id == apiResponseAssignedPledge.Id).ToList();
			}

		}

		[Test]
		public void ThenTheResponseDoesNotHaveAnError()
		{
			Assert.That(_apiResponse.HasError, Is.False);
		}

		[Test]
		public void ThenTheResponseContainsTheAssignedPledge()
		{
			var apiResponseAssignedPledge = (UserPledges)_apiResponse.Results;
			Assert.Multiple(() =>
			{
				Assert.That(apiResponseAssignedPledge.AssignerReference, Is.EqualTo(_user.UserReference));
				Assert.That(apiResponseAssignedPledge.AssigneeReference, Is.EqualTo(_user2.UserReference));
				Assert.That(apiResponseAssignedPledge.PledgeReference, Is.EqualTo(_request.PledgeReference));
				Assert.That(apiResponseAssignedPledge.Value, Is.EqualTo(_request.Value));
				Assert.That(apiResponseAssignedPledge.AdditionalInformation, Is.EqualTo(_request.AdditionalInformation));
				Assert.That(apiResponseAssignedPledge.AssigneeAccepted, Is.False);
				Assert.That(apiResponseAssignedPledge.AssigneeCompleted, Is.False);
				Assert.That(apiResponseAssignedPledge.AssignerSignedOff, Is.False);
			});
		}

		[Test]
		public void ThenAnAssignedPledgeIsPresent()
		{
			Assert.That(_allPlegdes, Has.Count.EqualTo(1));
		}
	}
}
