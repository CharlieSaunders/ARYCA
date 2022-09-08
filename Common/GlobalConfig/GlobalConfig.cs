using System.Net;
using System.Net.Sockets;

namespace Common.Config
{
	public interface IGlobalConfig
	{
		string GetApiUrl();
		string GetDefaultConnectionString();
		bool IsProduction();
		int MaximumAuditsPerUser();
	}

	public static class GlobalConfigFactory
	{
		public static IGlobalConfig For()
		{
			return new WindowsConfiguration();
		}
	}

	public class WindowsConfiguration : IGlobalConfig
	{
		private static string ApiUrl()
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
					return $"http://{ip}:9999";
			}
			throw new Exception("No network adapters with an IPv4 address in the system!");
		}

		private static string DefaultConnectionString => "Host = 192.168.0.3; Port=5432; Username=postgres; Password=PostgresPassword123; Database=ARYCA;";

		public string GetApiUrl() => ApiUrl();
		public string GetDefaultConnectionString() => DefaultConnectionString;
		public bool IsProduction() => false;
		public int MaximumAuditsPerUser() => 20;
	}
}
