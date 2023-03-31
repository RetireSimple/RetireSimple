using RetireSimple.Engine.Data.Analysis;

namespace RetireSimple.Tests.DomainModel {
	public class InvestmentTests : IDisposable {
		EngineDbContext Context { get; set; }

		public InvestmentTests() {
			Context = new EngineDbContext(
				new DbContextOptionsBuilder()
					.UseSqlite("Data Source=testing_inv.db")
					.Options);
			Context.Database.Migrate();
			Context.Database.EnsureCreated();
		}

		public void Dispose() {
			Context.Database.EnsureDeleted();
			Context.Dispose();
		}

		[Fact]
		public void TestStockInvestmentAdd() {
			var investment = new StockInvestment("") {
				StockPrice = 100,
				StockQuantity = 10,
				StockTicker = "TST"
			};

			Context.Portfolio.First(p => p.PortfolioId == 1).Investments.Add(investment);
			Context.SaveChanges();

			Assert.Equal(1, Context.Investment.Count());
			Assert.Single(Context.Portfolio.First(p => p.PortfolioId == 1).Investments);
			Assert.Single(Context.Profile.First(p => p.ProfileId == 1).Portfolios.First().Investments);
		}

		[Fact]
		public void TestStockInvestmentRemove() {
			var investment = new StockInvestment("") {
				StockPrice = 100,
				StockQuantity = 10,
				StockTicker = "TST"
			};

			Context.Portfolio.First(p => p.PortfolioId == 1).Investments.Add(investment);
			Context.SaveChanges();

			Context.Portfolio.First(p => p.PortfolioId == 1).Investments.Remove(investment);
			Context.SaveChanges();

			Assert.Equal(0, Context.Investment.Count());
			Assert.Empty(Context.Portfolio.First(p => p.PortfolioId == 1).Investments);
		}

		[Fact]
		public void TestInvestmentFKInvestmentModelConstraint() {
			var investment = new StockInvestment("") {
				StockPrice = 100,
				StockQuantity = 10,
				StockTicker = "TST"
			};

			var options = new OptionsDict();

			Context.Portfolio.First().Investments.Add(investment);
			Context.SaveChanges();

			Context.InvestmentModel.Add(new InvestmentModel() {
				InvestmentId = 1,
			});

			Context.SaveChanges();
			Action act = () => {
				Context.Investment.Remove(investment);
				Context.SaveChanges();
			};

			act.Should().NotThrow();
			Context.Investment.Should().BeEmpty();
			Context.InvestmentModel.Should().BeEmpty();
		}

		[Fact]
		public void TestInvestmentFKExpensesConstraint() {
			var investment = new StockInvestment("") {
				StockPrice = 100,
				StockQuantity = 10,
				StockTicker = "TST"
			};

			var options = new OptionsDict();

			Context.Portfolio.First().Investments.Add(investment);
			Context.SaveChanges();

			var testExpense1 = new OneTimeExpense { Amount = 100 };
			var testExpense2 = new OneTimeExpense { Amount = 200 };
			Context.Investment.First().Expenses.Add(testExpense1);
			Context.Investment.First().Expenses.Add(testExpense2);
			Context.SaveChanges();

			Action act = () => {
				Context.Investment.Remove(investment);
				Context.SaveChanges();
			};

			act.Should().NotThrow();
			Context.Investment.Should().BeEmpty();
			Context.Expense.Should().BeEmpty();
		}

		[Fact]
		public void TestInvestmentFKPortfolioConstraint() {
			var investment = new StockInvestment("") {
				StockPrice = 100,
				StockQuantity = 10,
				StockTicker = "TST"
			};

			Action act = () => {
				Context.Investment.Add(investment);
				Context.SaveChanges();
			};

			act.Should().Throw<DbUpdateException>();
		}

		[Fact]
		public void TestInvestmentFKTransfersFromConstraint() {
			var investment = new StockInvestment("") {
				StockPrice = 100,
				StockQuantity = 10,
				StockTicker = "TST"
			};
			var investment2 = new StockInvestment("") {
				StockPrice = 200,
				StockQuantity = 50,
				StockTicker = "TST2"
			};

			Context.Portfolio.First().Investments.Add(investment);
			Context.Portfolio.First().Investments.Add(investment2);
			Context.SaveChanges();

			var transfer = new InvestmentTransfer {
				SourceInvestment = Context.Investment.First(i => i.InvestmentId == 1),
				DestinationInvestment = Context.Investment.First(i => i.InvestmentId == 2)
			};
			Context.InvestmentTransfer.Add(transfer);
			Context.SaveChanges();

			Action act = () => {
				Context.Investment.Remove(investment);
				Context.SaveChanges();
			};

			act.Should().Throw<InvalidOperationException>();
		}

		[Fact]
		public void TestInvestmentFKTransfersConstraint() {
			var investment = new StockInvestment("") {
				StockPrice = 100,
				StockQuantity = 10,
				StockTicker = "TST"
			};
			var investment2 = new StockInvestment("") {
				StockPrice = 200,
				StockQuantity = 50,
				StockTicker = "TST2"
			};

			Context.Portfolio.First().Investments.Add(investment);
			Context.Portfolio.First().Investments.Add(investment2);
			Context.SaveChanges();

			var transfer = new InvestmentTransfer {
				DestinationInvestment = Context.Investment.First(i => i.InvestmentId == 1),
				SourceInvestment = Context.Investment.First(i => i.InvestmentId == 2)
			};
			Context.InvestmentTransfer.Add(transfer);
			Context.SaveChanges();

			Action act = () => {
				Context.Investment.Remove(investment);
				Context.SaveChanges();
			};

			act.Should().Throw<InvalidOperationException>();
		}

	}
}
