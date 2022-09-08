using System.ComponentModel.DataAnnotations;

namespace Common.DTO.Stocks
{

	public class StockPurchaseRequest
	{
		[Required]
		public string Symbol { get; set; }

		[Required]
		public decimal PurchasePrice { get; set; }

		[Required]
		public decimal Share { get; set; }

		public decimal GetTotalCost() => PurchasePrice * Share;
	}
}
