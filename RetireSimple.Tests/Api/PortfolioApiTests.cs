using RetireSimple.Engine.Api;

namespace RetireSimple.Tests.Api {
	public class PortfolioApiTests : IDisposable {
		private readonly EngineDbContext _context;
		private readonly PortfolioApi _portfolioApi;

		public PortfolioApiTests() {
			_context = new EngineDbContext(
				new DbContextOptionsBuilder()
					.UseSqlite("Data Source=testing_api_portfolio.db")
					.Options);
			_context.Database.Migrate();
			_context.Database.EnsureCreated();

			_portfolioApi = new(_context);
		}

		public void Dispose() {
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}

		[Fact]
		public void GetPortfolioUnknownIdThrowsException() {
			Action act = () => _portfolioApi.GetPortfolio(2);
			act.Should().Throw<InvalidOperationException>();
		}

		[Fact]
		public void GetEmptyPortfolioReturnsEmptyPortfolio() {
			var result = _portfolioApi.GetPortfolio(1);

			result.Investments.Should().BeEmpty();
			result.InvestmentVehicles.Should().BeEmpty();
		}

		[Fact]
		public void GetPortfolioReturnsPortfolioWithFilledFields() {
			//We are using the default portfolio
			var tempInvestment = new StockInvestment("");
			var invInVehicle = new BondInvestment("");

			var vehicle = new Vehicle401k();

			var portfolio = _context.Portfolio.First();

			vehicle.Investments.Add(invInVehicle);
			portfolio.InvestmentVehicles.Add(vehicle);
			portfolio.Investments.Add(tempInvestment);
			portfolio.Investments.Add(invInVehicle);
			_context.SaveChanges();

			var result = _portfolioApi.GetPortfolio(1);

			result.Investments.Should().ContainSingle();
			result.Investments.First().Should().BeEquivalentTo(tempInvestment);
			result.InvestmentVehicles.Should().ContainSingle();
			result.InvestmentVehicles.First().Should().BeEquivalentTo(vehicle);
		}

	}
}
