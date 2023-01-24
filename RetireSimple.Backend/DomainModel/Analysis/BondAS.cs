using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Investment;

namespace RetireSimple.Backend.DomainModel.Analysis {

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

			foreach(var k in investmentOptions.Keys) {
				newDict.TryAdd(k, investmentOptions[k]);
			}

			foreach(var k in BondAS.DefaultBondAnalysisOptions.Keys) {
				newDict.TryAdd(k, BondAS.DefaultBondAnalysisOptions[k]);
			}

			return newDict;
		}

	}
}