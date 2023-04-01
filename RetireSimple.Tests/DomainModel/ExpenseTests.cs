namespace RetireSimple.Tests.DomainModel {
	public class ExpenseTests : IDisposable {
		EngineDbContext Context { get; set; }

		public ExpenseTests() {
			Context = new EngineDbContext(
				new DbContextOptionsBuilder()
					.UseSqlite("Data Source=testing_expense.db")
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
		}

		public void Dispose() {
			Context.Database.EnsureDeleted();
			Context.Dispose();
		}

		[Fact]
		public void TestExpenseAdd() {
			//TODO: add manual constraint of Expense's investments
			var expense = new OneTimeExpense {
				Amount = 100.0,
				SourceInvestment = Context.Investment.First(i => i.InvestmentId == 1)
			};
			Context.Expense.Add(expense);
			Context.SaveChanges();
			Assert.Equal(1, Context.Expense.Count());

		}

		[Fact]
		public void TestExpenseRemove() {
			var expense = new OneTimeExpense {
				Amount = 100.0,
				SourceInvestment = Context.Investment.First(i => i.InvestmentId == 1)
			};
			Context.Expense.Add(expense);

			Context.SaveChanges();
			Context.Expense.Remove(expense);
			Context.SaveChanges();
			Assert.Equal(0, Context.Expense.Count());
		}

		[Fact]
		public void TestExpenseFKConstraint() {
			var expense = new OneTimeExpense { Amount = 100.0 };

			Action act = () => {
				Context.Expense.Add(expense);
				Context.SaveChanges();
			};

			act.Should().Throw<DbUpdateException>();
		}
	}
}
