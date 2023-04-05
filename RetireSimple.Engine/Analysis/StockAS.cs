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
			for (int i = 0; i < int.Parse(options["analysisLength"]); i++) {
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
		public static InvestmentModel MonteCarlo(StockInvestment investment, OptionsDict options) {
			var simPreset = ResolveMonteCarloPreset(investment, options);

			var priceSim = new MonteCarlo(simPreset, Utils.MonteCarlo.CreateRandomVariable(simPreset));
			var priceModel = priceSim.RunSimulation();
			var dividendModel = ProjectStockDividend(investment, DefaultStockAnalysisOptions);

			priceModel.MinModelData = priceModel.MinModelData.Zip(dividendModel, (price, dividend) => price * dividend).ToList();
			priceModel.AvgModelData = priceModel.AvgModelData.Zip(dividendModel, (price, dividend) => price * dividend).ToList();
			priceModel.MaxModelData = priceModel.MaxModelData.Zip(dividendModel, (price, dividend) => price * dividend).ToList();

			return priceModel;
		}

		public static OptionsDict ResolveMonteCarloPreset(StockInvestment investment, OptionsDict options) {
			var simPreset = options.GetValueOrDefault("MonteCarloPreset")
							?? investment.AnalysisOptionsOverrides.GetValueOrDefault("MonteCarloPreset")
							?? "DefaultStockAnalysis";
			var simOptions = new OptionsDict() {
				["basePrice"] = investment.StockPrice.ToString(),
				["analysisLength"] = options.GetValueOrDefault("analysisLength")
									?? investment.AnalysisOptionsOverrides.GetValueOrDefault("analysisLength")
									?? "60"
			};

			if (simPreset == "Custom") {
				//Extact the requisite options from the options dictionary and return them.
				simOptions["randomVariableType"] = options.GetValueOrDefault("randomVariableType") ??
													investment.AnalysisOptionsOverrides["randomVariableType"];
				simOptions["randomVariableMu"] = options.GetValueOrDefault("randomVariableMu") ??
													investment.AnalysisOptionsOverrides["randomVariableMu"];
				simOptions["randomVariableSigma"] = options.GetValueOrDefault("randomVariableSigma") ??
													investment.AnalysisOptionsOverrides["randomVariableSigma"];
				simOptions["randomVariableScaleFactor"] = options.GetValueOrDefault("randomVariableScaleFactor") ??
													investment.AnalysisOptionsOverrides["randomVariableScaleFactor"];
				simOptions["simCount"] = options.GetValueOrDefault("simCount") ??
													investment.AnalysisOptionsOverrides["simCount"];
			} else {
				var preset = ReflectionUtils.GetAnalysisPresets("MonteCarlo")[simPreset];
				simOptions = simOptions.Union(preset).ToDictionary(x => x.Key, x => x.Value);
			}
			return simOptions;
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
