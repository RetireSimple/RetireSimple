using RetireSimple.Engine.Data;

namespace RetireSimple.Engine.Api {
	public class PortfolioApi {
		private readonly EngineDbContext _context;

		public PortfolioApi(EngineDbContext context) {
			_context = context;
		}

		public PortfolioModel GetAnalysis(int id, OptionsDict? options) {
			var portfolio = _context.Portfolio.Find(id);
			if (portfolio is null) throw new InvalidOperationException("Portfolio not found");

			//TODO add smart update logic
			// if (portfolio.PortfolioModel is null){
			var analysis = portfolio.GenerateFullAnalysis();
			var invokeTime = DateTime.Now;
			portfolio.LastUpdated = invokeTime;
			if (portfolio.PortfolioModel is not null) {
				portfolio.PortfolioModel.LastUpdated = invokeTime;
				portfolio.PortfolioModel.AvgModelData = analysis.AvgModelData;
				portfolio.PortfolioModel.MinModelData = analysis.MinModelData;
				portfolio.PortfolioModel.MaxModelData = analysis.MaxModelData;
			}
			else {
				analysis.LastUpdated = invokeTime;
				portfolio.PortfolioModel = analysis;
			}

			_context.SaveChanges();
			// }


			var tempModel = portfolio.PortfolioModel;
			tempModel.MinModelData = tempModel.MinModelData.Select(d => Math.Max(d, 0)).ToList();
			tempModel.AvgModelData = tempModel.AvgModelData.Select(d => Math.Max(d, 0)).ToList();
			tempModel.MaxModelData = tempModel.MaxModelData.Select(d => Math.Max(d, 0)).ToList();

			return tempModel;
		}
	}
}
