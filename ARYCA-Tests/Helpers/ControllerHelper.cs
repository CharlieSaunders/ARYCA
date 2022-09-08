using Common.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Controllers;

namespace ARYCA_Tests.Helpers
{
	public static class ControllerHelper
	{
		public static HabitsController GetHabitController(DataContext dataContext, HttpContext context)
		{
			return new HabitsController(dataContext)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = context
				},
			};
		}

		public static PledgesController GetPledgesController(DataContext dataContext, HttpContext context)
		{
			return new PledgesController(dataContext)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = context
				},
			};
		}

		public static StocksController GetStocksController(DataContext dataContext, HttpContext context)
		{
			return new StocksController(dataContext)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = context
				},
			};
		}

		public static UnlockablesController GetUnlockablesController(DataContext dataContext, HttpContext context)
		{
			return new UnlockablesController(dataContext)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = context
				},
			};
		}

		public static ConfigController GetConfigController(DataContext dataContext, HttpContext context)
		{
			return new ConfigController(dataContext)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = context
				},
			};
		}

		public static AchievementsController GetAchievementsController(DataContext dataContext, HttpContext context)
		{
			return new AchievementsController(dataContext)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = context
				},
			};
		}
	}
}
