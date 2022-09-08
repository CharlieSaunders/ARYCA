using System.ComponentModel.DataAnnotations;

namespace Common.DTO.Stocks
{
	public class StockSellRequest
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string Symbol { get; set; }

		[Required]
		public decimal Shares { get; set; }

		[Required]
		public decimal SellPrice { get; set; }

		public decimal GetTotalCost() => Shares * SellPrice;
	}
}
