using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Investment;

namespace RetireSimple.Engine.Analysis {

	public class AnnuityAS {
		public static InvestmentModel DefaultAnnuityAnalyis(AnnuityInvestment investment, OptionsDict options) {
			throw new NotImplementedException();
		}

		public static readonly OptionsDict DefaultAnnuityAnalysisOptions = new() {
			["AnalysisLength"] = "60",                          //Number of months to project
			["AnnuityAnalysisExpectedGrowth"] = "0.1",            //Expected Percentage Growth of the stock

		};

		public static OptionsDict MergeAnalysisOption(AnnuityInvestment investment, OptionsDict dict) {
			var newDict = new OptionsDict(dict);
			var investmentOptions = investment.AnalysisOptionsOverrides;

			foreach(var k in investmentOptions.Keys) {
				newDict.TryAdd(k, investmentOptions[k]);
			}

			foreach(var k in DefaultAnnuityAnalysisOptions.Keys) {
				newDict.TryAdd(k, DefaultAnnuityAnalysisOptions[k]);
			}

			return newDict;
		}
	}
}
