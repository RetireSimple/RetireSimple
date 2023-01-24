using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Investment;

namespace RetireSimple.Backend.DomainModel.Analysis {

	public class SSAS {
		public static InvestmentModel DefaultSSAnalyis(SocialSecurityInvestment investment, OptionsDict options) {
			throw new NotImplementedException();
		}

		public static readonly OptionsDict DefaultSSAnalysisOptions = new() {
			["AnalysisLength"] = "60",                          //Number of months to project
			["SSAnalysisExpectedGrowth"] = "0.1",            //Expected Percentage Growth of the stock

		};

		public static OptionsDict SSAnalysisOption(InvestmentBase investment, OptionsDict dict) {
			var newDict = new OptionsDict(dict);
			var investmentOptions = investment.AnalysisOptionsOverrides;

			foreach(var k in investmentOptions.Keys) {
				newDict.TryAdd(k, investmentOptions[k]);
			}

			foreach(var k in SSAS.DefaultSSAnalysisOptions.Keys) {
				newDict.TryAdd(k, SSAS.DefaultSSAnalysisOptions[k]);
			}

			return newDict;
		}
	}
}
