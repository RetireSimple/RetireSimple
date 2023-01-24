using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Investment;

namespace RetireSimple.Backend.DomainModel.Analysis {

	public class AnnuityAS {
		public static InvestmentModel DefaultAnnuityAnalyis(AnnuityInvestment investment, OptionsDict options) {
			throw new NotImplementedException();
		}

		public static readonly OptionsDict DefaultAnnuityAnalysisOptions = new() {
			["AnalysisLength"] = "60",                          //Number of months to project
			["AnnuityAnalysisExpectedGrowth"] = "0.1",            //Expected Percentage Growth of the stock

		};

		public static OptionsDict AnnuityAnalysisOption(InvestmentBase investment, OptionsDict dict) {
			var newDict = new OptionsDict(dict);
			var investmentOptions = investment.AnalysisOptionsOverrides;

			foreach(var k in investmentOptions.Keys) {
				newDict.TryAdd(k, investmentOptions[k]);
			}

			foreach(var k in AnnuityAS.DefaultAnnuityAnalysisOptions.Keys) {
				newDict.TryAdd(k, AnnuityAS.DefaultAnnuityAnalysisOptions[k]);
			}

			return newDict;
		}
	}
}
