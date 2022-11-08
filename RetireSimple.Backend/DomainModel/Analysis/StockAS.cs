using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Investment;

namespace RetireSimple.Backend.DomainModel.Analysis {

	public class StockAS {


		public static InvestmentModel testAnalysis(StockInvestment investment, OptionsDict options) {
			Console.WriteLine("testAnalysis 1 invoked!");
			return new InvestmentModel() {
				InvestmentId = investment.InvestmentId,
				MaxModelData = new List<(double, double)>() { (0, 0) },
				MinModelData = new List<(double, double)>()
			};
		}

		public static InvestmentModel testAnalysis2(StockInvestment investment, OptionsDict options) {
			Console.WriteLine("testAnalysis 2 invoked!");
			return new InvestmentModel() {
				InvestmentId = investment.InvestmentId,
				MaxModelData = new List<(double, double)>(),
				MinModelData = new List<(double, double)>() { (0, 1), (1, 2) }
			};
		}
	}
}