using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Investment;

namespace RetireSimple.Engine.Analysis.Presets {

	internal class RegressionPresets {

		//large cap low uncertainty and growth
		//mid cap	medium uncertainty and growth
		//small cap	high uncertainty and growth
		//custom	user defined uncertainty and growth
		//high volatility	high uncertainty and crazy jumps between new highs and lows in a small period of time

		[AnalysisPreset(new string[] { nameof(StockAS.BinomialRegression) })]
		public static readonly OptionsDict LargeCap = new() {
			["percentGrowth"] = "0.0008",
			["uncertainty"] = "0.25",
		};

		[AnalysisPreset(new string[] { nameof(StockAS.BinomialRegression) })]
		public static readonly OptionsDict MidCap = new() {
			["percentGrowth"] = "0.0012",
			["uncertainty"] = "0.40",
		};

		[AnalysisPreset(new string[] { nameof(StockAS.BinomialRegression) })]
		public static readonly OptionsDict SmallCap = new() {
			["percentGrowth"] = "0.002",
			["uncertainty"] = "0.65",
		};

		[AnalysisPreset(new string[] { nameof(StockAS.BinomialRegression) })]
		public static readonly OptionsDict HighVolatility = new() {
			["percentGrowth"] = "0.0015",
			["uncertainty"] = "0.80",
		};

		//This makes no sense

		public static OptionsDict ResolveRegressionPreset(StockInvestment investment, OptionsDict options) {
			var simPreset = options.GetValueOrDefault("analysisPreset")
							?? investment.AnalysisOptionsOverrides.GetValueOrDefault("analysisPreset")
							?? "DefaultStockAnalysis";
			var simOptions = new OptionsDict() {
				["basePrice"] = investment.StockPrice.ToString(),
				["analysisLength"] = options.GetValueOrDefault("analysisLength")
									?? investment.AnalysisOptionsOverrides.GetValueOrDefault("analysisLength")
									?? "60",
			};
			if (simPreset == "Custom") {
				//Extact the requisite options from the options dictionary and return them.
				simOptions["percentGrowth"] = options.GetValueOrDefault("percentGrowth") ??
													investment.AnalysisOptionsOverrides["percentGrowth"];
				simOptions["uncertainty"] = options.GetValueOrDefault("uncertainty") ??
													investment.AnalysisOptionsOverrides["uncertainty"];
			} else {
				var preset = ReflectionUtils.GetAnalysisPresets("BinomialRegression")[simPreset];
				simOptions = simOptions.Union(preset).ToDictionary(x => x.Key, x => x.Value);
			}
			return simOptions;
		}
	}
}
