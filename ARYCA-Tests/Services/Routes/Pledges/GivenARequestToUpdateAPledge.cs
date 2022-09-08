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
	public sealed class GivenARequestToUpdateAPledge
	{

		private IServicesResponse _apiResponse;
		private PledgesController _pledgesController;
		private DataContext _datacontext;

		private User _user;
		private UpdatePledgeRequest _request;
		private Pledge _plegde;

		[OneTimeSetUp]
		public void Setup()
		{
			_user = UserHelper.GetActiveUser();

			_plegde = PledgeHelper.GetOldPledge();

			_request = new UpdatePledgeRequest
			{
				PledgeReference = _plegde.Reference,
				Name = "New Name",
				Details = "New Pledge Details"
			};

			using (_datacontext = DatabaseHelper.GetDatabase())
			{
				_datacontext = DatabaseHelper.SeedUsers(_datacontext, new List<User>() { _user });
				_datacontext = DatabaseHelper.SeedPledges(_datacontext, new List<Pledge>() { _plegde });

				_pledgesController = ControllerHelper.GetPledgesController(_datacontext, HttpContextHelper.GetWithArycaUserReference(_user.UserReference.ToString()));

				var subject = _pledgesController.Update(_request).Result as ObjectResult;
				_apiResponse = (IServicesResponse)subject.Value;
			}

		}

		[Test]
		public void ThenTheResponseDoesNotHaveAnError()
		{
			Assert.That(_apiResponse.HasError, Is.False);
		}

		[Test]
		public void ThenThePledgeReferenceWasNotUpdate()
		{
			var responsePledge = (Pledge)_apiResponse.Results;
			Assert.That(responsePledge.Reference, Is.EqualTo(_plegde.Reference));
		}

		[Test]
		public void ThenThePledgeNameWasUpdate()
		{
			var responsePledge = (Pledge)_apiResponse.Results;
			Assert.That(responsePledge.Name, Is.EqualTo(_request.Name));
		}

		[Test]
		public void ThenThePledgeDetailsWasUpdate()
		{
			var responsePledge = (Pledge)_apiResponse.Results;
			Assert.That(responsePledge.Details, Is.EqualTo(_request.Details));
		}
	}
}
