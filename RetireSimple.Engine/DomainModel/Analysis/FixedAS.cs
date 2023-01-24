using RetireSimple.Engine.DomainModel.Data;
using RetireSimple.Engine.DomainModel.Data.Investment;

namespace RetireSimple.Engine.DomainModel.Analysis {

	public class FixedAS {
		public static InvestmentModel DefaultFixedAnalyis(FixedInvestment investment, OptionsDict options) {
			throw new NotImplementedException();
		}

		public static readonly OptionsDict DefaultFixedAnalysisOptions = new() {
			["AnalysisLength"] = "60",                          //Number of months to project
			["FixedAnalysisExpectedGrowth"] = "0.1",            //Expected Percentage Growth of the stock

		};

		public static OptionsDict FixedAnalysisOption(InvestmentBase investment, OptionsDict dict) {
			var newDict = new OptionsDict(dict);
			var investmentOptions = investment.AnalysisOptionsOverrides;

			foreach (var k in investmentOptions.Keys) {
				newDict.TryAdd(k, investmentOptions[k]);
			}

			foreach (var k in DefaultFixedAnalysisOptions.Keys) {
				newDict.TryAdd(k, DefaultFixedAnalysisOptions[k]);
			}

			return newDict;
		}
	}
}