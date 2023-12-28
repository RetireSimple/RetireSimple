using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.User;

namespace RetireSimple.Engine.Api {
	public class PortfolioApi {
		private readonly EngineDbContext _context;

		public PortfolioApi(EngineDbContext context) {
			_context = context;
		}

		public PortfolioModel GetAnalysis(int id, OptionsDict? options) {
			var portfolio = _context.Portfolio.Find(id);
			if (portfolio is null) throw new InvalidOperationException("Portfolio not found");
			Console.WriteLine("Investments: " + portfolio.Investments.Count);

			if(portfolio.Investments.Count == 0){
				return new NullPortfolioModel();
			}
			else{
				var analysis = portfolio.GenerateFullAnalysis();
				var invokeTime = DateTime.Now;
				portfolio.LastUpdated = invokeTime;
				if (portfolio.PortfolioModel is not null) {
					portfolio.PortfolioModel.LastUpdated = invokeTime;
					portfolio.PortfolioModel.AvgModelData = analysis.AvgModelData;
					portfolio.PortfolioModel.MinModelData = analysis.MinModelData;
					portfolio.PortfolioModel.MaxModelData = analysis.MaxModelData;
				} else {
					analysis.LastUpdated = invokeTime;
					portfolio.PortfolioModel = analysis;
				}
				_context.SaveChanges();

				return analysis;
			}

		}

		public Portfolio GetPortfolio(int id) {
			var portfolio = _context.Portfolio.Find(id);
			if (portfolio is null) throw new InvalidOperationException("Portfolio not found");

			//Make sure investments only contain non-vehicle investments
			var vehicleInvestments = portfolio.InvestmentVehicles.SelectMany(v => v.Investments).ToList();
			portfolio.Investments = portfolio.Investments.Except(vehicleInvestments).ToList();

			//DO NOT SAVE CHANGES HERE
			return portfolio;
		}
	}
}
