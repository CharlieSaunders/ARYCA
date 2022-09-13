using Common.Classes;
using Common.Classes.ErrorHandling;
using Common.Classes.Investments;
using Common.Data;
using Common.DTO.Logs;
using Common.DTO.Stocks;
using Common.Entities.Investments;
using Common.Entities.Logs;
using Common.Entities.Users;
using Common.HttpClients;
using Infrastructure.EF;
using Infrastructure.EF.Investments;
using Infrastructure.Interfaces;

namespace Services.Routes
{
	public interface IStocksService
	{
		public Task<IServicesResponse> GetAllCrypto();
		public IServicesResponse GetUserStocks(User user);
		public IServicesResponse Purchase(User user, StockPurchaseRequest request);
		public IServicesResponse Sell(User user, StockSellRequest request);
		public IServicesResponse GetUserHistoricalStocks(User user);
	}

	public class StocksService : IStocksService
	{
		private readonly BinanceHttpClient _binanceHttpClient;

		private readonly EFUsersRepositories _inMemoryUserRepository;
		private readonly IInvestmentRepository _userStocksRepository;
		private readonly ILogsService _logsService;

		public StocksService(DataContext db)
		{
			_binanceHttpClient = new BinanceHttpClient();
			_inMemoryUserRepository = new EFUsersRepositories(db);
			_userStocksRepository = new EFInvestmentRepository(db);
			_logsService = new LogsService(db);
		}

		public async Task<IServicesResponse> GetAllCrypto()
		{
			var response = new IServicesResponse(new List<RawStockResponse>());

			try
			{
				response.Results = await _binanceHttpClient.GetAll();
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to all crypto", this.GetType()));
			}

			return response;
		}

		public IServicesResponse GetUserStocks(User user)
		{
			var response = new IServicesResponse(new List<UserInvestments>());
			try
			{
				response.Results = _userStocksRepository.GetAllForUser(user.UserReference);
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to get user stocks", this.GetType()));
			}

			return response;
		}

		public IServicesResponse GetUserHistoricalStocks(User user)
		{
			var response = new IServicesResponse(new List<UserHistoricalStocks>());
			try
			{
				response.Results = _inMemoryUserRepository.GetAllHistoricalStocks(user.UserReference);
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to get historical stocks", this.GetType()));
			}

			return response;
		}

		public IServicesResponse Purchase(User user, StockPurchaseRequest request)
		{
			var response = new IServicesResponse(new UserInvestments());
			try
			{
				var totalCost = request.GetTotalCost();
				var userBalance = user.Balance - totalCost;
				if (userBalance > 0)
				{
					var added = _userStocksRepository.Add(user.UserReference, request.Symbol, request.Share, request.PurchasePrice);
					if (!added.IsEmpty())
					{
						response.Results = added;
						_inMemoryUserRepository.UpdateUserBalance(user.UserReference, 0.0M - totalCost);
						_inMemoryUserRepository.AddTransaction(user.UserReference, UserTransactionType.StockPurchase, 0.0M - totalCost);
						_inMemoryUserRepository.AddTradeStat(user.UserReference);
					}
					else
						response.AddError(Error.Investments.UnableToAddInvestment, "Unable to purchase due to an error");
				}
				else
					response.AddError(Error.Users.UserBalanceBelowRequirements, "Unable to purchase due to user balance");
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to purchase stock", this.GetType()));
			}

			return response;
		}

		public IServicesResponse Sell(User user, StockSellRequest request)
		{
			var response = new IServicesResponse(new UserInvestments());
			try
			{
				var currInvestment = _userStocksRepository.Sell(user.UserReference, request.Shares, request.Id, request.SellPrice);
				var totalCost = request.GetTotalCost();

				if (!currInvestment.IsEmpty())
				{
					_inMemoryUserRepository.UpdateUserBalance(user.UserReference, totalCost);
					_inMemoryUserRepository.AddTransaction(user.UserReference, UserTransactionType.StockSold, totalCost);

					var profit = totalCost - (currInvestment.Price / currInvestment.Share * request.Shares);
					_inMemoryUserRepository.AddHistoricalStock(new RepositoryAddHistoricalStockRequest(user, request, profit));
					_inMemoryUserRepository.AddTradeProfitStat(user.UserReference, profit);
				}
			}
			catch (Exception ex)
			{
				response.AddError(ex);
				_logsService.Add(new CreateLogRequest(ex, Log.LogSeverity.Warning, "Unable to sell stock", this.GetType()));
			}

			return response;
		}
	}
}
