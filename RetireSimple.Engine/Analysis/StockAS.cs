using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Investment;

namespace RetireSimple.Engine.Analysis {

	public class StockAS {

		//This dictionary is statically available to allow for a common set of defaults that all
		//analysis modules for the same type of investment can use.
		public static readonly OptionsDict DefaultStockAnalysisOptions = new() {
			["AnalysisLength"] = "60",                          //Number of months to project
			["RandomVariableMu"] = "0",
			["RandomVariableSigma"] = "1",
			["RandomVariableScaleFactor"] = "1",
			["SimCount"] = "1000"
		};

		private static int GetDividendIntervalMonths(string interval) => interval switch {
			"Month" => 1,
			"Quarter" => 3,
			"Annual" => 12,
			_ => throw new ArgumentException("Invalid Dividend Interval")
		};

		/// <summary>
		/// Generates a list of monthly projected stock quantities using stock dividend distribution.
		/// First element of list is assumed to be for the current month/year.
		/// </summary>
		/// <param name="investment"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static List<decimal> ProjectStockDividend(StockInvestment investment, OptionsDict options) {
			var quantityList = new List<decimal>();
			var stockQuantity = investment.StockQuantity;
			var dividendPercent = investment.StockDividendPercent;
			var currentMonth = DateTime.Now.Month;
			var firstDividendMonth = investment.StockDividendFirstPaymentDate.Month;
			var monthInterval = GetDividendIntervalMonths(investment.StockDividendDistributionInterval);

			//quantityList.Add(stockQuantity);
			for (int i = 0; i < int.Parse(options["AnalysisLength"]); i++) {
				if ((currentMonth - firstDividendMonth) % monthInterval == 0) {
					stockQuantity += stockQuantity * dividendPercent;
				}
				quantityList.Add(stockQuantity);
				currentMonth++;
				if (currentMonth > 12) {
					currentMonth = 1;
				}
			}

			return quantityList;
		}

		[AnalysisModule("StockInvestment")]
		public static InvestmentModel MonteCarlo_NormalDist(StockInvestment investment, OptionsDict options) {
			var priceModel = MonteCarlo.MonteCarloSimNormal(investment, DefaultStockAnalysisOptions);
			//TODO Update to support other dividend types
			var dividendModel = ProjectStockDividend(investment, DefaultStockAnalysisOptions);

			priceModel.MinModelData = priceModel.MinModelData.Zip(dividendModel, (price, dividend) => price * dividend).ToList();
			priceModel.AvgModelData = priceModel.AvgModelData.Zip(dividendModel, (price, dividend) => price * dividend).ToList();
			priceModel.MaxModelData = priceModel.MaxModelData.Zip(dividendModel, (price, dividend) => price * dividend).ToList();

			return priceModel;
		}

		[AnalysisModule("StockInvestment")]
		public static InvestmentModel MonteCarlo_LogNormalDist(StockInvestment investment, OptionsDict options) {
			var priceModel = MonteCarlo.MonteCarloSimLogNormal(investment, DefaultStockAnalysisOptions);
			//TODO Update to support other dividend types
			var dividendModel = ProjectStockDividend(investment, DefaultStockAnalysisOptions);

			priceModel.MinModelData = priceModel.MinModelData.Zip(dividendModel, (price, dividend) => price * dividend).ToList();
			priceModel.AvgModelData = priceModel.AvgModelData.Zip(dividendModel, (price, dividend) => price * dividend).ToList();
			priceModel.MaxModelData = priceModel.MaxModelData.Zip(dividendModel, (price, dividend) => price * dividend).ToList();

			return priceModel;
		}

	}
}