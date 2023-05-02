using RetireSimple.Engine.Api;

namespace RetireSimple.Tests.Api {
	public class ExpensesApiTests : IDisposable {

		private readonly EngineDbContext context;
		private readonly ExpensesApi api;

		public ExpensesApiTests() {
			context = new EngineDbContext(
				new DbContextOptionsBuilder()
					.UseSqlite("Data Source=testing_api_expense.db")
					.Options);
			context.Database.Migrate();
			context.Database.EnsureCreated();

			api = new ExpensesApi(context);
		}

		public void Dispose() {
			context.Database.EnsureDeleted();
			context.Dispose();
		}

		[Fact]
		public void AddExpense_NoInvestmentFound_ThrowsArgumentException() {
			Assert.Throws<ArgumentException>(() => api.Add(1, new OptionsDict()));
		}

		[Fact]
		public void AddExpense_UnknownType_ThrowsArgumentException(){
			var investment = new StockInvestment("") {
				PortfolioId = 1,
				InvestmentData = new OptionsDict() {
					{ "symbol", "AAPL" }
				}
			};
			context.Investment.Add(investment);
			context.SaveChanges();

			var expenseData = new OptionsDict() {
				{ "amount", "100" },
				{ "expenseType", "Unknown" }
			};

			Assert.Throws<ArgumentException>(() => api.Add(1, expenseData));
		}

		public static readonly IEnumerable<object[]> TestExpenseData = new List<object[]>(){
			new object[] {
				new OptionsDict() {
					{ "amount", "100" },
					{ "expenseType", "Recurring" },
					{ "frequency", "1" },
					{ "startDate", "1/1/2021" },
					{ "endDate", "1/1/2022" }
				}
			},
			new object[] {
				new OptionsDict() {
					{ "amount", "100" },
					{ "expenseType", "OneTime" },
					{ "date", "1/1/2021" }
				}
			}
		};

		[Theory, MemberData(nameof(TestExpenseData))]
		public void AddExpense_ValidExpenseData_CreatesCorrectExpense(OptionsDict expenseData) {
			var investment = new StockInvestment("") {
				PortfolioId = 1,
				InvestmentData = new OptionsDict() {
					{ "symbol", "AAPL" }
				}
			};
			context.Investment.Add(investment);
			context.SaveChanges();

			var expenseId = api.Add(1, expenseData);
			var expense = context.Expense.Find(expenseId) ?? throw new ArgumentException("Expense not found");

			expense.Amount.ToString().Should().Be(expenseData["amount"]);
			expense.Should().BeOfType(expenseData["expenseType"] == "Recurring" ? typeof(RecurringExpense) : typeof(OneTimeExpense));
			expense.ExpenseData.Should().HaveCount(expenseData.Count - 2);
			expense.ExpenseData.Should().IntersectWith(expenseData);
		}

		[Fact]
		public void RemoveExpense_ExpenseNotFound_ThrowsArgumentException() {
			Assert.Throws<ArgumentException>(() => api.Remove(1));
		}

		[Fact]
		public void RemoveExpense_ExpenseFound_RemovesExpense() {
			var investment = new StockInvestment("") {
				PortfolioId = 1,
				InvestmentData = new OptionsDict() {
					{ "symbol", "AAPL" }
				}
			};
			context.Investment.Add(investment);
			context.SaveChanges();

			var expense = new RecurringExpense() {
				Amount = 100,
				SourceInvestmentId = 1,
				ExpenseData = new OptionsDict() {
					{ "frequency", "1" },
					{ "startDate", "1/1/2021" },
					{ "endDate", "1/1/2022" }
				}
			};
			context.Expense.Add(expense);
			context.SaveChanges();

			api.Remove(expense.ExpenseId);

			context.Expense.Should().BeEmpty();
		}

		[Fact]
		public void UpdateExpense_ExpenseNotFound_ThrowsArgumentException() {
			Assert.Throws<ArgumentException>(() => api.Update(1, new OptionsDict()));
		}

		[Fact]
		public void UpdateExpense_ExpenseFound_UpdatesExpense() {
			var investment = new StockInvestment("") {
				PortfolioId = 1,
				InvestmentData = new OptionsDict() {
					{ "symbol", "AAPL" }
				}
			};
			context.Investment.Add(investment);
			context.SaveChanges();

			var expense = new RecurringExpense() {
				Amount = 100,
				SourceInvestmentId = 1,
				ExpenseData = new OptionsDict() {
					{ "frequency", "1" },
					{ "startDate", "1/1/2021" },
					{ "endDate", "1/1/2022" }
				}
			};
			context.Expense.Add(expense);
			context.SaveChanges();

			var newExpenseData = new OptionsDict() {
				{ "amount", "200" },
				{"frequency", "2" },
				{ "startDate", "1/1/2022" },
				{ "endDate", "1/1/2023" }
			};
			api.Update(expense.ExpenseId, newExpenseData);

			var updatedExpense = context.Expense.Find(expense.ExpenseId) ?? throw new ArgumentException("Expense not found");

			updatedExpense.Amount.ToString().Should().Be(newExpenseData["amount"]);
			updatedExpense.ExpenseData.Should().HaveCount(newExpenseData.Count - 1);
			updatedExpense.ExpenseData.Should().IntersectWith(newExpenseData);
		}


		[Fact]
		public void GetExpense_InvestmentNotFound_ThrowsArgumentException() {
			Assert.Throws<ArgumentException>(() => api.GetExpenses(1));
		}

		[Fact]
		public void GetExpenses_InvestmentExists_ReturnsAllExpensesForInvestment(){
			var investment = new StockInvestment("") {
				PortfolioId = 1,
				InvestmentData = new OptionsDict() {
					{ "symbol", "AAPL" }
				}
			};
			context.Investment.Add(investment);
			context.SaveChanges();

			var expense1 = new RecurringExpense() {
				Amount = 100,
				SourceInvestmentId = 1,
				ExpenseData = new OptionsDict() {
					{ "frequency", "1" },
					{ "startDate", "1/1/2021" },
					{ "endDate", "1/1/2022" }
				}
			};
			var expense2 = new OneTimeExpense() {
				Amount = 200,
				SourceInvestmentId = 1,
				ExpenseData = new OptionsDict() {
					{ "date", "1/1/2021" }
				}
			};
			context.Expense.AddRange(expense1, expense2);
			context.SaveChanges();

			var expenses = api.GetExpenses(1);

			expenses.Should().HaveCount(2);
			expenses.Should().Contain(expense1);
			expenses.Should().Contain(expense2);
		}
	}
}
