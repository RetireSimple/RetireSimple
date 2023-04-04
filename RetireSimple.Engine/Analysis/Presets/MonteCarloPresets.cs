using RetireSimple.Engine.Data;

namespace RetireSimple.Engine.Analysis.Presets {
	public static class MonteCarloPresets {
		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo) })]
		public static readonly OptionsDict DefaultStockAnalysis = new() {
			["analysisLength"] = "60",                          //Number of months to project
			["randomVariableMu"] = "0",
			["randomVariableSigma"] = "1",
			["randomVariableScaleFactor"] = "1",
			["simCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo) })]
		public static readonly OptionsDict LargeCapGrowth = new() {
			["analysisLength"] = "60",
			["randomVariableMu"] = "0.1",
			["randomVariableSigma"] = "0.2",
			["randomVariableScaleFactor"] = "1",
			["simCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo) })]
		public static readonly OptionsDict LargeCapValue = new() {
			["analysisLength"] = "60",
			["randomVariableMu"] = "0.05",
			["randomVariableSigma"] = "0.2",
			["randomVariableScaleFactor"] = "1",
			["simCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo) })]
		public static readonly OptionsDict SmallCapGrowth = new() {
			["analysisLength"] = "60",
			["randomVariableMu"] = "0.15",
			["randomVariableSigma"] = "0.3",
			["randomVariableScaleFactor"] = "1",
			["simCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo) })]
		public static readonly OptionsDict SmallCapValue = new() {
			["analysisLength"] = "60",
			["randomVariableMu"] = "0.1",
			["randomVariableSigma"] = "0.3",
			["randomVariableScaleFactor"] = "1",
			["simCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo) })]
		public static readonly OptionsDict MidCapGrowth = new() {
			["analysisLength"] = "60",
			["randomVariableMu"] = "0.1",
			["randomVariableSigma"] = "0.25",
			["randomVariableScaleFactor"] = "1",
			["simCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo) })]
		public static readonly OptionsDict MidCapValue = new() {
			["analysisLength"] = "60",
			["randomVariableMu"] = "0.05",
			["randomVariableSigma"] = "0.25",
			["randomVariableScaleFactor"] = "1",
			["simCount"] = "1000"
		};

		[AnalysisPreset(new string[] { nameof(StockAS.MonteCarlo) })]
		public static readonly OptionsDict InternationalStock = new() {
			["analysisLength"] = "60",
			["randomVariableMu"] = "0.1",
			["randomVariableSigma"] = "0.3",
			["randomVariableScaleFactor"] = "1",
			["simCount"] = "1000"
		};
	}
}