using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Investment;

namespace RetireSimple.Engine.Analysis.Presets {
	public static class MonteCarloPresets {
		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo) })]
		public static readonly OptionsDict DefaultStockAnalysis = new() {
			["randomVariableType"] = "Normal",
			["randomVariableMu"] = "0",
			["randomVariableSigma"] = "1",
			["randomVariableScaleFactor"] = "1",
			["simCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo) })]
		public static readonly OptionsDict LargeCapGrowth = new() {
			["randomVariableType"] = "Normal",
			["randomVariableMu"] = "0.1",
			["randomVariableSigma"] = "0.2",
			["randomVariableScaleFactor"] = "1",
			["simCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo) })]
		public static readonly OptionsDict LargeCapValue = new() {
			["randomVariableType"] = "Normal",
			["randomVariableMu"] = "0.05",
			["randomVariableSigma"] = "0.2",
			["randomVariableScaleFactor"] = "1",
			["simCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo) })]
		public static readonly OptionsDict SmallCapGrowth = new() {
			["randomVariableType"] = "Normal",
			["randomVariableMu"] = "0.15",
			["randomVariableSigma"] = "0.3",
			["randomVariableScaleFactor"] = "1",
			["simCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo) })]
		public static readonly OptionsDict SmallCapValue = new() {
			["randomVariableType"] = "Normal",
			["randomVariableMu"] = "0.1",
			["randomVariableSigma"] = "0.3",
			["randomVariableScaleFactor"] = "1",
			["simCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo) })]
		public static readonly OptionsDict MidCapGrowth = new() {
			["randomVariableType"] = "Normal",
			["randomVariableMu"] = "0.1",
			["randomVariableSigma"] = "0.25",
			["randomVariableScaleFactor"] = "1",
			["simCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo) })]
		public static readonly OptionsDict MidCapValue = new() {
			["randomVariableType"] = "Normal",
			["randomVariableMu"] = "0.05",
			["randomVariableSigma"] = "0.25",
			["randomVariableScaleFactor"] = "1",
			["simCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo) })]
		public static readonly OptionsDict InternationalStock = new() {
			["randomVariableType"] = "Normal",
			["randomVariableMu"] = "0.1",
			["randomVariableSigma"] = "0.3",
			["randomVariableScaleFactor"] = "1",
			["simCount"] = "1000"
		};

		public static OptionsDict ResolveMonteCarloPreset(StockInvestment investment, OptionsDict options) {
			var simPreset = options.GetValueOrDefault("analysisPreset")
							?? investment.AnalysisOptionsOverrides.GetValueOrDefault("monteCarloPreset")
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
	}
}