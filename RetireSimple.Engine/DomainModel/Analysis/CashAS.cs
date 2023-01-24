using RetireSimple.Engine.DomainModel.Data;
using RetireSimple.Engine.DomainModel.Data.Investment;

namespace RetireSimple.Engine.DomainModel.Analysis {
	public class CashAS {
		public static InvestmentModel DefaultCashAnalysis(CashInvestment investment, OptionsDict options) {
			throw new NotImplementedException();
		}

		public static readonly OptionsDict DefaultCashAnalysisOptions = new() {
			["AnalysisLength"] = "60",                          //Number of months to project
			["CashAnalysisExpectedGrowth"] = "0.1",            //Expected Percentage Growth of the stock

		};

		public static OptionsDict CashAnalysisOption(InvestmentBase investment, OptionsDict dict) {
			var newDict = new OptionsDict(dict);
			var investmentOptions = investment.AnalysisOptionsOverrides;

			foreach (var k in investmentOptions.Keys) {
				newDict.TryAdd(k, investmentOptions[k]);
			}

			foreach (var k in DefaultCashAnalysisOptions.Keys) {
				newDict.TryAdd(k, DefaultCashAnalysisOptions[k]);
			}

			return newDict;
		}

	}
}
