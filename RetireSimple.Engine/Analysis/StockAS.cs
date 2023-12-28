using RetireSimple.Engine.Analysis.Presets;
using RetireSimple.Engine.Analysis.Utils;
using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Investment;

namespace RetireSimple.Engine.Analysis {

	public class StockAS {

		//This dictionary is statically available to allow for a common set of defaults that all
		//analysis modules for the same type of investment can use.
		public static readonly OptionsDict DefaultStockAnalysisOptions = new() {
			["analysisLength"] = "60",                          //Number of months to project
			["randomVariableMu"] = "0",
			["randomVariableSigma"] = "1",
			["randomVariableScaleFactor"] = "1",
			["simCount"] = "1000"
		};

		public static readonly OptionsDict DefaultStockAnalysisOptionsRegression = new() {
			["analysisLength"] = "60",                          //Number of months to project
			["percentGrowth"] = "0.012",
			["uncertainty"] = "0.50"
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


			//Why are we doing it like this
			//Why not use formulas???

			//quantityList.Add(stockQuantity);
			for (int i = 0; i < int.Parse(options["analysisLength"]); i++) {
				//if current month - first dividend month is a multiple of the monthly interval then...
				if ((currentMonth - firstDividendMonth) % monthInterval == 0) {
					//new stock quantity = previous stock quantity * (1 + dividend percent)
					stockQuantity += stockQuantity * dividendPercent;
				}
				//add stock quanity to the list
				quantityList.Add(stockQuantity);
				//increment the current month
				currentMonth++;
				//if the month is going to be greater than 12 reset it to 1
				if (currentMonth > 12) {
					currentMonth = 1;
				}
			}
			//return the list different stock quanity
			return quantityList;
		}

		[AnalysisModule("StockInvestment")]
		public static InvestmentModel BinomialRegression(StockInvestment investment, OptionsDict options) {
			var simPreset = RegressionPresets.ResolveRegressionPreset(investment, options);

			var priceSim = new Regression(simPreset);
			var pricemodel = priceSim.RunSimulation();
			var dividendModel = ProjectStockDividend(investment, DefaultStockAnalysisOptionsRegression);

			pricemodel.MinModelData = pricemodel.MinModelData.Zip(dividendModel, (price, dividend) => price * dividend).ToList();
			pricemodel.AvgModelData = pricemodel.AvgModelData.Zip(dividendModel, (price, dividend) => price * dividend).ToList();
			pricemodel.MaxModelData = pricemodel.MaxModelData.Zip(dividendModel, (price, dividend) => price * dividend).ToList();

			return pricemodel;
		}

		[AnalysisModule("StockInvestment")]
		public static InvestmentModel MonteCarlo(StockInvestment investment, OptionsDict options) {
			var simPreset = MonteCarloPresets.ResolveMonteCarloPreset(investment, options);

			var priceSim = new MonteCarlo(simPreset);
			var priceModel = priceSim.RunSimulation();
			var dividendModel = ProjectStockDividend(investment, DefaultStockAnalysisOptions);

			priceModel.MinModelData = priceModel.MinModelData.Zip(dividendModel, (price, dividend) => price * dividend).ToList();
			priceModel.AvgModelData = priceModel.AvgModelData.Zip(dividendModel, (price, dividend) => price * dividend).ToList();
			priceModel.MaxModelData = priceModel.MaxModelData.Zip(dividendModel, (price, dividend) => price * dividend).ToList();

			return priceModel;
		}

#if DEBUG
		[AnalysisModule("StockInvestment")]
		public static InvestmentModel DummyStock(StockInvestment investment, OptionsDict options) {
			var model = new InvestmentModel() {
				MinModelData = new List<decimal>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
				AvgModelData = new List<decimal>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
				MaxModelData = new List<decimal>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
			};
			return model;
		}
#endif

	}

}
