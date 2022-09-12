using Common.Classes;
using Newtonsoft.Json;
namespace ARYCA_Tests.Helpers
{
	public static class HttpClientResponseHelper
	{
		public static IServicesResponse For(IServicesResponse initValue)
		{
			var serialised = JsonConvert.SerializeObject(initValue);
			var unserialised = JsonConvert.DeserializeObject<IServicesResponse>(serialised);
			return unserialised;
		}
	}
}
