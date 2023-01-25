namespace RetireSimple.Tests.DomainModel {
	public class PortfolioTests : IDisposable {
		EngineDbContext context { get; set; }

		private readonly ITestOutputHelper output;

		public PortfolioTests(ITestOutputHelper output) {
			context = new EngineDbContext(
				new DbContextOptionsBuilder()
					.UseSqlite("Data Source=testing_portfolio.db")
					.Options);
			context.Database.Migrate();
			context.Database.EnsureCreated();

			this.output = output;
		}

		public void Dispose() {
			context.Database.EnsureDeleted();
			context.Dispose();
		}

		[Fact]
		public void TestPortfolioFKConstraintDelete() {
			var portfolio = new Portfolio();
			portfolio.PortfolioName = "test";
			context.Profile.First(p => p.ProfileId == 1).Portfolios.Add(portfolio);

			var investment = new StockInvestment("testAnalysis");
			investment.StockPrice = 100;
			investment.StockQuantity = 10;
			investment.StockTicker = "TST";

			portfolio.Investments.Add(investment);
			context.SaveChanges();

			Action act = () => {
				context.Portfolio.Remove(portfolio);
				context.SaveChanges();
			};


			act.Should().NotThrow();
		}

		[Fact]
		public void TestPortfolioFKConstraintProfile() {
			var portfolio = new Portfolio();
			portfolio.PortfolioName = "test";

			Action act = () => {
				context.Portfolio.Add(portfolio);
				context.SaveChanges();
			};

			act.Should().Throw<DbUpdateException>();
		}

		[Fact]
		public void TestPortfolioAdd() {
			var portfolio = new Portfolio();
			portfolio.PortfolioName = "test";
			var portfolio2 = new Portfolio();
			portfolio2.PortfolioName = "test2";

			context.Profile.First(p => p.ProfileId == 1).Portfolios.Add(portfolio);
			context.Profile.First(p => p.ProfileId == 1).Portfolios.Add(portfolio2);

			context.SaveChanges();
			context.Portfolio.Should().HaveCount(3);
		}

		[Fact]
		public void TestPortfolioRemove() {
			var portfolio = new Portfolio();
			portfolio.PortfolioName = "test";
			var portfolio2 = new Portfolio();
			portfolio2.PortfolioName = "test2";

			context.Profile.First(p => p.ProfileId == 1).Portfolios.Add(portfolio);
			context.Profile.First(p => p.ProfileId == 1).Portfolios.Add(portfolio2);
			context.SaveChanges();
			context.Portfolio.Remove(portfolio);
			context.SaveChanges();

			context.Portfolio.Should().HaveCount(2);
		}
	}
}
