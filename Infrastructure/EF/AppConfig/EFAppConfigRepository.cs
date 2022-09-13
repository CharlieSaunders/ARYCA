using Common.Data;
using Common.Entities.Configuration;
using Infrastructure.Interfaces;

namespace Infrastructure.EF.AppConfig
{
	public class EFAppConfigurationRepository : IApplicationConfigurationRepository
	{
		private readonly DataContext _db;
		private readonly decimal APP_BALANCE_ADJUSTMENT = 0.02M;

		public EFAppConfigurationRepository(DataContext db)
		{
			_db = db;
		}

		public ApplicationConfiguration Get()
		{
			var configs = _db.AppConfig.ToList();
			if (configs.Any())
				return configs.First();
			else
				return new ApplicationConfiguration();
		}

		public ApplicationConfiguration Create()
		{
			var newAppconfig = new ApplicationConfiguration();
			newAppconfig.ApplicationName = "ARYCA";
			newAppconfig.ImageText = "Any Request You Could Ask";
			newAppconfig.LoginImageUrl = "default-background.jpg";
			newAppconfig.MemorableDate = DateTime.Now;

			_db.AppConfig.Add(newAppconfig);
			_db.SaveChanges();

			return newAppconfig;
		}

		public ApplicationConfiguration Update(string appName, DateTime date, string imageText, string imageUrl)
		{
			var config = Get();
			config.ApplicationName = appName;
			config.MemorableDate = date;
			config.ImageText = imageText;
			config.LoginImageUrl = imageUrl;
			_db.SaveChanges();
			return config;
		}

		public ApplicationConfiguration UpdateBalance(decimal change)
		{
			var config = Get();
			change *= APP_BALANCE_ADJUSTMENT;
			config.Balance += change;
			_db.SaveChanges();
			return config;
		}
	}
}
