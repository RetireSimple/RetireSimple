namespace RetireSimple.Tests.DomainModel {
	public class InvestmentModelTests : IDisposable {
		EngineDbContext Context { get; set; }

		public InvestmentModelTests() {
			Context = new EngineDbContext(
				new DbContextOptionsBuilder()
					.UseSqlite("Data Source=testing_invmodel.db")
					.Options);
			Context.Database.Migrate();
			Context.Database.EnsureCreated();

			var investment = new StockInvestment("test") {
				StockPrice = 100,
				StockQuantity = 10,
				StockTicker = "TST"
			};
			Context.Portfolio.First(p => p.PortfolioId == 1).Investments.Add(investment);
			Context.SaveChanges();
		}

		public void Dispose() {
			Context.Database.EnsureDeleted();
			Context.Dispose();
		}

		[Fact]
		public void TestInvestmentModelAdd() {
			var model = new InvestmentModel {
				Investment = Context.Portfolio.First(p => p.PortfolioId == 1).Investments.First(i => i.InvestmentId == 1)
			};

			Context.InvestmentModel.Add(model);
			Context.SaveChanges();

			Assert.Single(Context.InvestmentModel);
		}

		[Fact]
		public void TestInvestmentModelRemove() {
			var model = new InvestmentModel {
				Investment = Context.Portfolio.First(p => p.PortfolioId == 1).Investments.First(i => i.InvestmentId == 1)
			};

			Context.InvestmentModel.Add(model);
			Context.SaveChanges();

			Context.InvestmentModel.Remove(model);
			Context.SaveChanges();

			Assert.Equal(0, Context.InvestmentModel.Count());
		}

		[Fact]
		public void TestInvestmentModelFKConstraint() {
			var model = new InvestmentModel();

			Action act = () => {
				Context.InvestmentModel.Add(model);
				Context.SaveChanges();
			};

			act.Should().Throw<DbUpdateException>();
		}
	}
}
