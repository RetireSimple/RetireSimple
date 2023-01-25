using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Investment;

namespace RetireSimple.Engine.Analysis {

	public class BondAS {

		public static InvestmentModel DefaultBondAnalysis(BondInvestment investment, OptionsDict options) {
			throw new NotImplementedException();
		}

		public static readonly OptionsDict DefaultBondAnalysisOptions = new() {
			["AnalysisLength"] = "60",                          //Number of months to project
			["BondAnalysisExpectedGrowth"] = "0.1",            //Expected Percentage Growth of the stock

		};

		public static OptionsDict BondAnalysisOption(InvestmentBase investment, OptionsDict dict) {
			var newDict = new OptionsDict(dict);
			var investmentOptions = investment.AnalysisOptionsOverrides;

			foreach (var k in investmentOptions.Keys) {
				newDict.TryAdd(k, investmentOptions[k]);
			}

			foreach (var k in DefaultBondAnalysisOptions.Keys) {
				newDict.TryAdd(k, DefaultBondAnalysisOptions[k]);
			}

			return newDict;
		}

	}
}