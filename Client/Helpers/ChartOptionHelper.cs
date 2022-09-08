using ApexCharts;
using Common.Entities.Users;

namespace Client.Helpers
{
	public static class ChartOptionHelper
	{
		public static ApexChartOptions<UserHistoricalStocks> GetStocksChartOptions()
		{
			var _chartOptions = new ApexChartOptions<UserHistoricalStocks>
			{
				Theme = new Theme
				{
					Mode = Mode.Dark,
					Palette = PaletteType.Palette1
				}
			};

			_chartOptions.Yaxis = new List<YAxis>();

			_chartOptions.Yaxis.Add(new YAxis
			{
				Title = new AxisTitle { Text = "Trades" },
				DecimalsInFloat = 0,
				Opposite = true
			});

			_chartOptions.Yaxis.Add(new YAxis
			{
				Title = new AxisTitle { Text = "Profit" },
				DecimalsInFloat = 3
			});

			return _chartOptions;
		}
	}
}
