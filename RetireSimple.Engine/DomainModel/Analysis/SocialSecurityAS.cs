using RetireSimple.Engine.DomainModel.Data;
using RetireSimple.Engine.DomainModel.Data.Investment;

namespace RetireSimple.Engine.DomainModel.Analysis {
	public class SocialSecurityAS {
		public static InvestmentModel DefaultSocialSecurityAnalysis(SocialSecurityInvestment investment, OptionsDict options) {
			throw new NotImplementedException();
		}

		public static readonly OptionsDict DefaultSocialSecurityAnalysisOptions = new() {
			["AnalysisLength"] = "60",                          //Number of months to project
			["SocialSecurityAnalysisExpectedGrowth"] = "0.1",            //Expected Percentage Growth of the stock

		};

		public static OptionsDict SocialSecurityAnalysisOption(InvestmentBase investment, OptionsDict dict) {
			var newDict = new OptionsDict(dict);
			var investmentOptions = investment.AnalysisOptionsOverrides;

			foreach (var k in investmentOptions.Keys) {
				newDict.TryAdd(k, investmentOptions[k]);
			}

			foreach (var k in DefaultSocialSecurityAnalysisOptions.Keys) {
				newDict.TryAdd(k, DefaultSocialSecurityAnalysisOptions[k]);
			}

			return newDict;
		}

	}
}
