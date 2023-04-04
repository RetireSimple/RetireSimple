using RetireSimple.Engine.Data;

namespace RetireSimple.Engine.Analysis.Presets {
	public static class MonteCarloPresets {
		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo_NormalDist),
										nameof(StockAS.MonteCarlo_LogNormalDist) })]
		public static readonly OptionsDict DefaultStockAnalysisOptions = new() {
			["AnalysisLength"] = "60",                          //Number of months to project
			["RandomVariableMu"] = "0",
			["RandomVariableSigma"] = "1",
			["RandomVariableScaleFactor"] = "1",
			["SimCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo_NormalDist),
										nameof(StockAS.MonteCarlo_LogNormalDist) })]
		public static readonly OptionsDict LargeCapGrowthStockAnalysisOptions = new() {
			["AnalysisLength"] = "60",
			["RandomVariableMu"] = "0.1",
			["RandomVariableSigma"] = "0.2",
			["RandomVariableScaleFactor"] = "1",
			["SimCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo_NormalDist),
										nameof(StockAS.MonteCarlo_LogNormalDist) })]
		public static readonly OptionsDict LargeCapValueStockAnalysisOptions = new() {
			["AnalysisLength"] = "60",
			["RandomVariableMu"] = "0.05",
			["RandomVariableSigma"] = "0.2",
			["RandomVariableScaleFactor"] = "1",
			["SimCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo_NormalDist),
										nameof(StockAS.MonteCarlo_LogNormalDist) })]
		public static readonly OptionsDict SmallCapGrowthStockAnalysisOptions = new() {
			["AnalysisLength"] = "60",
			["RandomVariableMu"] = "0.15",
			["RandomVariableSigma"] = "0.3",
			["RandomVariableScaleFactor"] = "1",
			["SimCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo_NormalDist),
										nameof(StockAS.MonteCarlo_LogNormalDist) })]
		public static readonly OptionsDict SmallCapValueStockAnalysisOptions = new() {
			["AnalysisLength"] = "60",
			["RandomVariableMu"] = "0.1",
			["RandomVariableSigma"] = "0.3",
			["RandomVariableScaleFactor"] = "1",
			["SimCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo_NormalDist),
										nameof(StockAS.MonteCarlo_LogNormalDist) })]
		public static readonly OptionsDict MidCapGrowthStockAnalysisOptions = new() {
			["AnalysisLength"] = "60",
			["RandomVariableMu"] = "0.1",
			["RandomVariableSigma"] = "0.25",
			["RandomVariableScaleFactor"] = "1",
			["SimCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo_NormalDist),
										nameof(StockAS.MonteCarlo_LogNormalDist) })]
		public static readonly OptionsDict MidCapValueStockAnalysisOptions = new() {
			["AnalysisLength"] = "60",
			["RandomVariableMu"] = "0.05",
			["RandomVariableSigma"] = "0.25",
			["RandomVariableScaleFactor"] = "1",
			["SimCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo_NormalDist),
										nameof(StockAS.MonteCarlo_LogNormalDist) })]
		public static readonly OptionsDict InternationalStockAnalysisOptions = new() {
			["AnalysisLength"] = "60",
			["RandomVariableMu"] = "0.1",
			["RandomVariableSigma"] = "0.3",
			["RandomVariableScaleFactor"] = "1",
			["SimCount"] = "1000"
		};
	}
}