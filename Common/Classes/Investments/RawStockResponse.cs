namespace Common.Classes.Investments
{

	public class RawStockResponse
	{

		public RawStockResponse()
		{

		}

		public string symbol { get; set; }
		public string priceChange { get; set; }
		public string priceChangePercent { get; set; }
		public string weightedAvgPrice { get; set; }
		public string prevClosePrice { get; set; }
		public string lastPrice { get; set; }
		public string lastQty { get; set; }
		public string bidPrice { get; set; }
		public string bidQty { get; set; }
		public string askPrice { get; set; }
		public string askQty { get; set; }
		public string openPrice { get; set; }
		public string highPrice { get; set; }
		public string lowPrice { get; set; }
		public string volume { get; set; }
		public string quoteVolume { get; set; }
		public long openTime { get; set; }
		public long closeTime { get; set; }
		public long firstId { get; set; }
		public long lastId { get; set; }
		public long count { get; set; }

		public decimal GetPrice()
		{
			var split = askPrice.Split(".");
			var decimals = $"{split[1][0]}{split[1][2]}{split[1][3]}";
			var newResponse = $"{split[0]}.{decimals}";
			return decimal.Parse(newResponse);
		}

		public decimal GetPriceChange()
		{
			var split = priceChange.Split(".");
			var decimals = $"{split[1][0]}{split[1][2]}{split[1][3]}";
			var newResponse = $"{split[0]}.{decimals}";
			return decimal.Parse(newResponse);
		}

		public decimal GetOpenPrice()
		{
			var split = openPrice.Split(".");
			var decimals = $"{split[1][0]}{split[1][2]}{split[1][3]}";
			var newResponse = $"{split[0]}.{decimals}";
			return decimal.Parse(newResponse);
		}

		public decimal GetLastClosePrice()
		{
			var split = prevClosePrice.Split(".");
			var decimals = $"{split[1][0]}{split[1][2]}{split[1][3]}";
			var newResponse = $"{split[0]}.{decimals}";
			return decimal.Parse(newResponse);
		}

		public decimal GetBasicPrice()
		{
			var currentPrice = decimal.Parse(askPrice);
			return 1 / currentPrice;
		}
	}
}
