using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Investment;

namespace RetireSimple.Backend.DomainModel.Analysis {

	public class StockAS {


		public static InvestmentModel testAnalysis(StockInvestment investment, OptionsDict options) {
			var value = investment.StockPrice * investment.StockQuantity;

			//HACK I think this code is leaky, but it won't exist in final builds
			//Just PoC for Investments

			

			return new InvestmentModel() {
				InvestmentId = investment.InvestmentId,
				MaxModelData = new List<(decimal, decimal)>() {
					(0, value),
					(1, 2*value),
					(2, 4*value)
				},
				MinModelData = new List<(decimal, decimal)>() {
					(0, value),
					(1, new decimal(0.5) * value),
					(2, new decimal(0.25) * value)
				}
			};
		}

		public static InvestmentModel testAnalysis2(StockInvestment investment, OptionsDict options) {
			var value = investment.StockPrice * investment.StockQuantity;

			return new InvestmentModel() {
				InvestmentId = investment.InvestmentId,
				MaxModelData = new List<(decimal, decimal)>() {
					(0, value),
					(1, new decimal(1.5)*value),
					(2, 2*value)
				},
				MinModelData = new List<(decimal, decimal)>() {
					(0, value),
					(1, new decimal(0.75) * value),
					(2, new decimal(0.5) * value)
				}
			};
		}
	}
}