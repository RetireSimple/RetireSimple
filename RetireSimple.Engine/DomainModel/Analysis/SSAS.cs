using RetireSimple.Engine.DomainModel.Data;
using RetireSimple.Engine.DomainModel.Data.Investment;

namespace RetireSimple.Engine.DomainModel.Analysis {

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

			foreach (var k in investmentOptions.Keys) {
				newDict.TryAdd(k, investmentOptions[k]);
			}

			foreach (var k in DefaultSSAnalysisOptions.Keys) {
				newDict.TryAdd(k, DefaultSSAnalysisOptions[k]);
			}

			return newDict;
		}
	}
}
