namespace RetireSimple.Tests.DomainModel.Sqlite {
    public class ExpenseTests : IDisposable {

        InvestmentDBContext context { get; set; }

        private readonly ITestOutputHelper output;

        public ExpenseTests(ITestOutputHelper output) {
            context = new InvestmentDBContext(
                new DbContextOptionsBuilder()
                    .UseSqlite("Data Source=InvestmentDB_expensetests.db")
                    .Options);
            context.Database.Migrate();
            context.Database.EnsureCreated();

            this.output = output;

            var investment = new StockInvestment("test");
            investment.StockPrice = 100;
            investment.StockQuantity = 10;
            investment.StockTicker = "TST";
            context.Portfolio.First(p => p.PortfolioId == 1).Investments.Add(investment);
            context.SaveChanges();
        }

        public void Dispose() {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void TestExpenseAdd() {
            //TODO: add manual constraint of Expense's investments
            var expense = new OneTimeExpense();
            expense.Amount = 100.0;

            expense.SourceInvestment = context.Investment.First(i => i.InvestmentId == 1);
            context.Expense.Add(expense);
            context.SaveChanges();
            Assert.Equal(1, context.Expense.Count());

        }

        [Fact]
        public void TestExpenseRemove() {
            var expense = new OneTimeExpense();
            expense.Amount = 100.0;

            expense.SourceInvestment = context.Investment.First(i => i.InvestmentId == 1);
            context.Expense.Add(expense);

            context.SaveChanges();
            context.Expense.Remove(expense);
            context.SaveChanges();
            Assert.Equal(0, context.Expense.Count());
        }

        [Fact]
        public void TestExpenseFKConstraint() {
            var expense = new OneTimeExpense();
            expense.Amount = 100.0;

            Action act = () => {
                context.Expense.Add(expense);
                context.SaveChanges();
            };

            act.Should().Throw<DbUpdateException>();
        }
    }
}
