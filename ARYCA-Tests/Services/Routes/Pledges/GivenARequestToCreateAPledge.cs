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
	public sealed class GivenARequestToCreateAPledge
	{
		private IServicesResponse _apiResponse;
		private PledgesController _pledgesController;
		private DataContext _datacontext;

		private User _user;
		private CreatePledgeRequest _request;
		private Pledge _returnedPledge;
		private List<Pledge> _pledgesSubject;

		[OneTimeSetUp]
		public void Setup()
		{
			_user = UserHelper.GetActiveUser();

			_request = new CreatePledgeRequest
			{
				Name = "New Plegde",
				Details = "New Pledge Details"
			};

			using (_datacontext = DatabaseHelper.GetDatabase())
			{
				_datacontext = DatabaseHelper.SeedUsers(_datacontext, new List<User>() { _user });

				_pledgesController = ControllerHelper.GetPledgesController(_datacontext, HttpContextHelper.GetWithArycaUserReference(_user.UserReference.ToString()));

				var subject = _pledgesController.Create(_request).Result as ObjectResult;
				_apiResponse = (IServicesResponse)subject.Value;

				_returnedPledge = (Pledge)_apiResponse.Results;
				_pledgesSubject = _datacontext.Pledges.Where(x => x.Reference == _returnedPledge.Reference).ToList();
			}
		}

		[Test]
		public void ThenTheResponseDoesNotHaveAnError()
		{
			Assert.That(_apiResponse.HasError, Is.False);
		}

		[Test]
		public void ThenThePledgeWasCreated()
		{
			Assert.That(_pledgesSubject, Has.Count.EqualTo(1));
		}

		[Test]
		public void ThenTheApiResponseReturnsTheCreatedPledge()
		{
			Assert.Multiple(() =>
			{
				Assert.That(_returnedPledge.Name, Is.EqualTo(_request.Name));
				Assert.That(_returnedPledge.Details, Is.EqualTo(_request.Details));
				Assert.That(_returnedPledge.Reference, !Is.EqualTo(Guid.Empty));
			});
		}
	}
}
