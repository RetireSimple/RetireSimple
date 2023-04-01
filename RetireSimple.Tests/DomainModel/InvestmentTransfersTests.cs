namespace RetireSimple.Tests.DomainModel {
	public class InvestmentTransfersTests : IDisposable {
		EngineDbContext Context { get; set; }

		public InvestmentTransfersTests() {
			Context = new EngineDbContext(
				new DbContextOptionsBuilder()
					.UseSqlite("Data Source=testing_transfer.db")
					.Options);
			Context.Database.Migrate();
			Context.Database.EnsureCreated();

			var investment = new StockInvestment("") {
				StockPrice = 100,
				StockQuantity = 10,
				StockTicker = "TST"
			};
			Context.Portfolio.First(p => p.PortfolioId == 1).Investments.Add(investment);
			Context.SaveChanges();
			var investment2 = new StockInvestment("") {
				StockPrice = 100,
				StockQuantity = 10,
				StockTicker = "TST"
			};
			Context.Portfolio.First(p => p.PortfolioId == 1).Investments.Add(investment2);
			Context.SaveChanges();
		}

		public void Dispose() {
			Context.Database.EnsureDeleted();
			Context.Dispose();
		}

		[Fact]
		public void TestInvestmentTransferAdd() {
			var transfer = new InvestmentTransfer {
				SourceInvestment = Context.Investment.First(i => i.InvestmentId == 1),
				DestinationInvestment = Context.Investment.First(i => i.InvestmentId == 2)
			};
			Context.InvestmentTransfer.Add(transfer);
			Context.SaveChanges();

			Assert.Single(Context.InvestmentTransfer);
		}

		[Fact]
		public void TestInvestmentTransferRemove() {
			var transfer = new InvestmentTransfer {
				SourceInvestment = Context.Investment.First(i => i.InvestmentId == 1),
				DestinationInvestment = Context.Investment.First(i => i.InvestmentId == 2)
			};
			Context.InvestmentTransfer.Add(transfer);
			Context.SaveChanges();

			Context.InvestmentTransfer.Remove(transfer);
			Context.SaveChanges();

			Assert.Empty(Context.InvestmentTransfer);
		}

		[Fact]
		public void TestInvestmentTransferFKConstraint() {
			var transfer = new InvestmentTransfer();
			Action act = () => {
				Context.InvestmentTransfer.Add(transfer);
				Context.SaveChanges();
			};

			act.Should().Throw<DbUpdateException>();
		}

	}
}
