using System;
using ARYCA_Tests.Helpers;
using ARYCA_Tests.Helpers.TypeHelpers;
using Client.ServicesBridge;
using Client.States.Toast;
using Common.Classes;
using Common.Entities.Configuration;
using Common.Entities.Habits;
using Common.HttpClients;
using Moq;
using NUnit.Framework;

namespace ARYCA_Tests.Clients.Bridges.Habits
{
	[TestFixture]
	public sealed class GivenAClientRequestToGetAllHabits
	{

		private List<Habit> _subject;
		private List<Habit> _habits;
		private HabitsBridge _habitsBridge;

		[OneTimeSetUp]
		public async Task Setup()
		{
			_habits = HabitHelper.GetCompletableHabitList();

			var user = UserHelper.GetActiveUser();
			var httpClientResponse = HttpClientResponseHelper.For(new IServicesResponse(_habits));
			var httpClient = new Mock<IGenericHttpClient>();

			httpClient.Setup(x => x.GetAsyncConvertResult(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(httpClientResponse);

			_habitsBridge = new HabitsBridge(httpClient.Object, new ToasterService());
			_subject = await _habitsBridge.GetAllHabits(user.Username);
		}

		[Test]
		public void TheTheHabitsAreNotNull()
		{
			Assert.That(_subject, Is.Not.Null);
		}

		[Test]
		public void ThenTheHabitsAreReturned()
		{
			Assert.That(_subject.Count, Is.EqualTo(_habits.Count));
		}
	}
}

