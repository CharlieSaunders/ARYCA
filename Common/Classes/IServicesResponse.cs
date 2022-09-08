using Common.Classes.ErrorHandling;
using System.Net;

namespace Common.Classes
{
	public class IServicesResponse
	{
		public bool HasError { get; set; }
		public dynamic Results { get; set; }
		public List<Error> Errors { get; set; }

		public IServicesResponse(dynamic initialValue)
		{
			HasError = false;
			Results = initialValue;
			Errors = new List<Error>();
		}

		public IServicesResponse()
		{
			Errors = new List<Error>();
		}

		public IServicesResponse(Exception ex) => AddError(ex);

		public void AddError(Error error)
		{
			HasError = true;
			Errors.Add(error);
		}

		public void AddError(string technicalMessage, string userMessage)
		{
			HasError = true;
			Errors.Add(new Error(userMessage, technicalMessage, (int)HttpStatusCode.BadRequest));
		}

		public void AddError(Exception ex)
		{
			HasError = true;
			Errors.Add(new Error("An unknown error occurred", ex.Message, (int)HttpStatusCode.InternalServerError));
		}
	}
}
