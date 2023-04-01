namespace RetireSimple.Tests.DomainModel {
	public class PortfolioTests : IDisposable {
		EngineDbContext Context { get; set; }

		public PortfolioTests() {
			Context = new EngineDbContext(
				new DbContextOptionsBuilder()
					.UseSqlite("Data Source=testing_portfolio.db")
					.Options);
			Context.Database.Migrate();
			Context.Database.EnsureCreated();
		}

		public void Dispose() {
			Context.Database.EnsureDeleted();
			Context.Dispose();
		}

		[Fact]
		public void TestPortfolioFKConstraintDelete() {
			var portfolio = new Portfolio {
				PortfolioName = "test"
			};
			Context.Profile.First(p => p.ProfileId == 1).Portfolios.Add(portfolio);

			var investment = new StockInvestment("") {
				StockPrice = 100,
				StockQuantity = 10,
				StockTicker = "TST"
			};

			portfolio.Investments.Add(investment);
			Context.SaveChanges();

			Action act = () => {
				Context.Portfolio.Remove(portfolio);
				Context.SaveChanges();
			};


			act.Should().NotThrow();
		}

		[Fact]
		public void TestPortfolioFKConstraintProfile() {
			var portfolio = new Portfolio { PortfolioName = "test" };

			Action act = () => {
				Context.Portfolio.Add(portfolio);
				Context.SaveChanges();
			};

			act.Should().Throw<DbUpdateException>();
		}

		[Fact]
		public void TestPortfolioAdd() {
			var portfolio = new Portfolio { PortfolioName = "test" };
			var portfolio2 = new Portfolio { PortfolioName = "test2" };

			Context.Profile.First(p => p.ProfileId == 1).Portfolios.Add(portfolio);
			Context.Profile.First(p => p.ProfileId == 1).Portfolios.Add(portfolio2);

			Context.SaveChanges();
			Context.Portfolio.Should().HaveCount(3);
		}

		[Fact]
		public void TestPortfolioRemove() {
			var portfolio = new Portfolio { PortfolioName = "test" };
			var portfolio2 = new Portfolio { PortfolioName = "test2" };

			Context.Profile.First(p => p.ProfileId == 1).Portfolios.Add(portfolio);
			Context.Profile.First(p => p.ProfileId == 1).Portfolios.Add(portfolio2);
			Context.SaveChanges();
			Context.Portfolio.Remove(portfolio);
			Context.SaveChanges();

			Context.Portfolio.Should().HaveCount(2);
		}
	}
}
