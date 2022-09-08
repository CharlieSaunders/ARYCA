using Common.Classes;
using Common.Data;
using Common.DTO.Stocks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using Services.Routes;

namespace Services.Controllers
{
	[Route("public/[controller]")]
	public class StocksController : ControllerBase
	{
		private readonly IStocksService _stocksService;
		private readonly IRequestHelper _requestHelper;

		public StocksController(DataContext db)
		{
			_requestHelper = new RequestHelper(db);
			_stocksService = new StocksService(db);
		}

		[HttpGet]
		[Authorize]
		public ActionResult<IServicesResponse> GetAll()
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _stocksService.GetAllCrypto().Result, this);
		}

		[HttpGet("Purchased")]
		[Authorize]
		public ActionResult<IServicesResponse> GetUserStocks()
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _stocksService.GetUserStocks(user), this);
		}

		[HttpPut("Purchase")]
		[Authorize]
		public ActionResult<IServicesResponse> PurchaseStock([FromBody] StockPurchaseRequest request)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _stocksService.Purchase(user, request), this);
		}

		[HttpPut("Sell")]
		[Authorize]
		public ActionResult<IServicesResponse> SellStock([FromBody] StockSellRequest request)
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _stocksService.Sell(user, request), this);
		}

		[HttpGet("Historical")]
		[Authorize]
		public ActionResult<IServicesResponse> GetAllHistorical()
		{
			var user = _requestHelper.UserFrom(Request);
			return ServicesResponseHandler.From(user, _stocksService.GetUserHistoricalStocks(user), this);
		}
	}
}
