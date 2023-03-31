using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Investment;

namespace RetireSimple.Engine.Analysis {

	public class FixedAS {
		public static InvestmentModel DefaultFixedAnalyis(FixedInvestment investment, OptionsDict options) {
			throw new NotImplementedException();
		}

		public static readonly OptionsDict DefaultFixedAnalysisOptions = new() {
			["AnalysisLength"] = "60",                          //Number of months to project
		};

		public static OptionsDict MergeAnalysisOptions(FixedInvestment investment, OptionsDict dict) {
			var newDict = new OptionsDict(dict);
			var investmentOptions = investment.AnalysisOptionsOverrides;

			foreach(var k in investmentOptions.Keys) {
				newDict.TryAdd(k, investmentOptions[k]);
			}

			foreach(var k in DefaultFixedAnalysisOptions.Keys) {
				newDict.TryAdd(k, DefaultFixedAnalysisOptions[k]);
			}

			return newDict;
		}
	}
}