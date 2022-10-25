using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Investment;

namespace RetireSimple.Backend.DomainModel.Analysis {
	public delegate InvestmentModel StockAnalysisDelegate(StockInvestment investment);

	public class StockAS{
		
		
		public static InvestmentModel testAnalysis(StockInvestment investment) {
			Console.WriteLine("testAnalysis 1 invoked!");
			return new InvestmentModel();
		}

		public static InvestmentModel testAnalysis2(StockInvestment investment) {
			Console.WriteLine("testAnalysis 2 invoked!");
			return new InvestmentModel();
		}
	}
}