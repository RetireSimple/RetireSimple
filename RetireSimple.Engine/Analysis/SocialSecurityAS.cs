using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Investment;

namespace RetireSimple.Engine.Analysis {
	public class SocialSecurityAS {
		public static InvestmentModel DefaultSocialSecurityAnalysis(SocialSecurityInvestment investment, OptionsDict options) {
			throw new NotImplementedException();
		}

		public static readonly OptionsDict DefaultSocialSecurityAnalysisOptions = new() {
			["AnalysisLength"] = "60",                          //Number of months to project
		};

		public static OptionsDict MergeAnalysisOptions(SocialSecurityInvestment investment, OptionsDict dict) {
			var newDict = new OptionsDict(dict);
			var investmentOptions = investment.AnalysisOptionsOverrides;

			foreach(var k in investmentOptions.Keys) {
				newDict.TryAdd(k, investmentOptions[k]);
			}

			foreach(var k in DefaultSocialSecurityAnalysisOptions.Keys) {
				newDict.TryAdd(k, DefaultSocialSecurityAnalysisOptions[k]);
			}

			return newDict;
		}

	}
}
