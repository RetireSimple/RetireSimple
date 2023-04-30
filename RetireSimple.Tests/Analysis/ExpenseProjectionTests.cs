using RetireSimple.Engine.Analysis.Utils;
using RetireSimple.Engine.Data.Analysis;

namespace RetireSimple.Tests.Analysis {
	public class ExpenseProjectionTests : IDisposable {
		private readonly EngineDbContext context;

		public ExpenseProjectionTests() {
			context = new EngineDbContext(
				new DbContextOptionsBuilder()
					.UseSqlite("Data Source=testing_expense_project.db")
					.Options);
			context.Database.Migrate();
			context.Database.EnsureCreated();

			var investment = new StockInvestment("") {
				PortfolioId = 1,
				InvestmentData = new OptionsDict() {
					{ "symbol", "AAPL" }
				}
			};

			context.Investment.Add(investment);
			context.SaveChanges();
		}

		public void Dispose() {
			context.Database.EnsureDeleted();
			context.Dispose();
		}

		[Fact]
		public void ProjectExpenses_NoExpenses_ReturnsAllZeroes() {
			var expenses = ExpenseUtils.ProjectExpenses(context, 1, 12);
			expenses.Should().BeEquivalentTo(Enumerable.Repeat(0M, 12));
		}

		[Fact]
		public void ProjectExpenses_SingleOneTime_ProjectsExpected() {
			var expense = new OneTimeExpense() {
				SourceInvestmentId = 1,
				Amount = 100,
				ExpenseType = "OneTime",
				Date = DateOnly.FromDateTime(DateTime.Now.AddMonths(6))
			};
			context.Expense.Add(expense);
			context.SaveChanges();

			var expenses = ExpenseUtils.ProjectExpenses(context, 1, 12);
			expenses.Should().BeEquivalentTo(new List<decimal>() {
				0, 0, 0, 0, 0, 0, 100, 100, 100, 100, 100, 100
			});
		}

		[Fact]
		public void ProjectExpenses_SingleRecurring_ProjectsExpected() {
			var expense = new RecurringExpense() {
				SourceInvestmentId = 1,
				Amount = 100,
				ExpenseType = "Recurring",
				Frequency = 1,
				StartDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(6)),
				EndDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(12))
			};
			context.Expense.Add(expense);
			context.SaveChanges();

			var expenses = ExpenseUtils.ProjectExpenses(context, 1, 12);
			expenses.Should().BeEquivalentTo(new List<decimal>() {
				0, 0, 0, 0, 0, 0, 100, 200, 300, 400, 500, 600
			});
		}

		[Fact]

		public void ProjectExpenses_MixedTypes_ProjectsExpected() {
			var expense1 = new OneTimeExpense() {
				SourceInvestmentId = 1,
				Amount = 100,
				ExpenseType = "OneTime",
				Date = DateOnly.FromDateTime(DateTime.Now.AddMonths(6))
			};
			var expense2 = new RecurringExpense() {
				SourceInvestmentId = 1,
				Amount = 100,
				ExpenseType = "Recurring",
				Frequency = 1,
				StartDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(6)),
				EndDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(12))
			};
			context.Expense.Add(expense1);
			context.Expense.Add(expense2);
			context.SaveChanges();

			var expenses = ExpenseUtils.ProjectExpenses(context, 1, 12);
			expenses.Should().BeEquivalentTo(new List<decimal>() {
				0, 0, 0, 0, 0, 0, 200, 300, 400, 500, 600, 700
			});
		}

		[Fact]
		public void ApplyExpenses_ModelIsAltered() {
			var model = new InvestmentModel() {
				AvgModelData = Enumerable.Repeat(200M, 12).ToList(),
				MinModelData = Enumerable.Repeat(200M, 12).ToList(),
				MaxModelData = Enumerable.Repeat(200M, 12).ToList(),
			};

			var expenses = Enumerable.Range(0, 12).Select(i => i * 10M).ToList();

			ExpenseUtils.ApplyExpenses(ref model, expenses);

			model.AvgModelData.Should().BeEquivalentTo(new List<decimal>() {
				200, 190, 180, 170, 160, 150, 140, 130, 120, 110, 100, 90
			});
			model.MinModelData.Should().BeEquivalentTo(new List<decimal>() {
				200, 190, 180, 170, 160, 150, 140, 130, 120, 110, 100, 90
			});
			model.MaxModelData.Should().BeEquivalentTo(new List<decimal>() {
				200, 190, 180, 170, 160, 150, 140, 130, 120, 110, 100, 90
			});
		}

		[Fact]
		public void ApplyExpenses_ExpensesExceedValue_FloorsToZero(){
			var model = new InvestmentModel() {
				AvgModelData = Enumerable.Repeat(100M, 12).ToList(),
				MinModelData = Enumerable.Repeat(100M, 12).ToList(),
				MaxModelData = Enumerable.Repeat(100M, 12).ToList(),
			};

			var expenses = Enumerable.Range(0, 12).Select(i => i * 10M).ToList();

			ExpenseUtils.ApplyExpenses(ref model, expenses);

			model.AvgModelData.Should().BeEquivalentTo(new List<decimal>(){
				100, 90, 80, 70, 60, 50, 40, 30, 20, 10, 0, 0
			});
			model.MinModelData.Should().BeEquivalentTo(new List<decimal>(){
				100, 90, 80, 70, 60, 50, 40, 30, 20, 10, 0, 0
			});
			model.MaxModelData.Should().BeEquivalentTo(new List<decimal>(){
				100, 90, 80, 70, 60, 50, 40, 30, 20, 10, 0, 0
			});
		}

	}
}