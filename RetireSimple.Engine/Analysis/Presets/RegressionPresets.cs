using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Investment;

namespace RetireSimple.Engine.Analysis.Presets {

	internal class RegressionPresets {
		[AnalysisPreset(new string[] { nameof(StockAS.SimpleRegression) })]
		public static readonly OptionsDict EnergySector = new() {
			["percentGrowth"] = "0.012",
			["uncertainty"] = "0.50",
		};

		[AnalysisPreset(new string[] { nameof(StockAS.SimpleRegression) })]
		public static readonly OptionsDict FinancialSector = new() {
			["percentGrowth"] = "0.010",
			["uncertainty"] = "0.30",
		};

		[AnalysisPreset(new string[] { nameof(StockAS.SimpleRegression) })]
		public static readonly OptionsDict HealthCareSector = new () {
			["percentGrowth"] = "0.06",
			["uncertainty"] = "0.25",
		};

		[AnalysisPreset(new string[] { nameof(StockAS.SimpleRegression) })]
		public static readonly OptionsDict TechnologySector = new () {
			["percentGrowth"] = "0.14",
			["uncertainty"] = "0.40",
		};

		[AnalysisPreset(new string[] { nameof(StockAS.SimpleRegression) })]
		public static readonly OptionsDict ConsumerDiscretionarySector = new () {
			["percentGrowth"] = "0.05",
			["uncertainty"] = "0.25",
		};

		[AnalysisPreset(new string[] { nameof(StockAS.SimpleRegression) })]
		public static readonly OptionsDict ConsumerStaplesSector = new() {
			["percentGrowth"] = "0.012",
			["uncertainty"] = "0.50",
		};
		[AnalysisPreset(new string[] { nameof(StockAS.SimpleRegression) })]
		public static readonly OptionsDict MaterialsSector = new() {
			["percentGrowth"] = "0.07",
			["uncertainty"] = "0.10",
		};
		[AnalysisPreset(new string[] { nameof(StockAS.SimpleRegression) })]
		public static readonly OptionsDict CommunicationServicesSector = new () {
			["percentGrowth"] = "0.09",
			["uncertainty"] = "0.20",
		};
		[AnalysisPreset(new string[] { nameof(StockAS.SimpleRegression) })]
		public static readonly OptionsDict IndustrialsSector = new() {
			["percentGrowth"] = "0.10",
			["uncertainty"] = "0.50",
		};
		[AnalysisPreset(new string[] { nameof(StockAS.SimpleRegression) })]
		public static readonly OptionsDict UtilitiesSector = new() {
			["percentGrowth"] = "0.03",
			["uncertainty"] = "0.10",
		};
		[AnalysisPreset(new string[] { nameof(StockAS.SimpleRegression) })]
		public static readonly OptionsDict RealEstateSector = new() {
			["percentGrowth"] = "0.05",
			["uncertainty"] = "0.50",
		};

		public static OptionsDict ResolveRegressionPreset(StockInvestment investment, OptionsDict options) {
			var simPreset = options.GetValueOrDefault("analysisPreset")
							?? investment.AnalysisOptionsOverrides.GetValueOrDefault("analysisPreset")
							?? "DefaultStockAnalysis";
			var simOptions = new OptionsDict() {
				["basePrice"] = investment.StockPrice.ToString(),
				["analysisLength"] = options.GetValueOrDefault("analysisLength")
									?? investment.AnalysisOptionsOverrides.GetValueOrDefault("analysisLength")
									?? "60"
			};
			if (simPreset == "Custom") {
				//Extact the requisite options from the options dictionary and return them.
				simOptions["percentGrowth"] = options.GetValueOrDefault("percentGrowth") ??
													investment.AnalysisOptionsOverrides["percentGrowth"];
				simOptions["uncertainty"] = options.GetValueOrDefault("uncertainty") ??
													investment.AnalysisOptionsOverrides["uncertainty"];
			} else {
				var preset = ReflectionUtils.GetAnalysisPresets("SimpleRegression")[simPreset];
				simOptions = simOptions.Union(preset).ToDictionary(x => x.Key, x => x.Value);
			}
			return simOptions;
		}
	}
}
