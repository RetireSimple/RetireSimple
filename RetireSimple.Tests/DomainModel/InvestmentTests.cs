namespace RetireSimple.Tests.DomainModel
{
    public class InvestmentTests : IDisposable
    {
        InvestmentDBContext context { get; set; }

        private readonly ITestOutputHelper output;

        public InvestmentTests(ITestOutputHelper output)
        {
            context = new InvestmentDBContext(
                new DbContextOptionsBuilder()
                    .UseSqlite("Data Source=InvestmentDB_invtests.db")
                    .Options);
            context.Database.Migrate();
            context.Database.EnsureCreated();

            this.output = output;
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void TestStockInvestmentAdd()
        {
            var investment = new StockInvestment("testAnalysis");
            investment.StockPrice = 100;
            investment.StockQuantity = 10;
            investment.StockTicker = "TST";

            context.Portfolio.First(p => p.PortfolioId == 1).Investments.Add(investment);
            context.SaveChanges();

            Assert.Equal(1, context.Investment.Count());
            Assert.Single(context.Portfolio.First(p => p.PortfolioId == 1).Investments);
            Assert.Single(context.Profile.First(p => p.ProfileId == 1).Portfolios.First().Investments);
        }

        [Fact]
        public void TestStockInvestmentRemove()
        {
            var investment = new StockInvestment("testAnalysis");
            investment.StockPrice = 100;
            investment.StockQuantity = 10;
            investment.StockTicker = "TST";

            context.Portfolio.First(p => p.PortfolioId == 1).Investments.Add(investment);
            context.SaveChanges();

            context.Portfolio.First(p => p.PortfolioId == 1).Investments.Remove(investment);
            context.SaveChanges();

            Assert.Equal(0, context.Investment.Count());
            Assert.Empty(context.Portfolio.First(p => p.PortfolioId == 1).Investments);
        }

        [Fact]
        public void TestInvestmentFKInvestmentModelConstraint()
        {
            var investment = new StockInvestment("testAnalysis");
            investment.StockPrice = 100;
            investment.StockQuantity = 10;
            investment.StockTicker = "TST";

            var options = new OptionsDict();

            context.Portfolio.First().Investments.Add(investment);
            context.SaveChanges();

            context.InvestmentModel.Add(investment.InvokeAnalysis(options));

            context.SaveChanges();
            Action act = () =>
            {
                context.Investment.Remove(investment);
                context.SaveChanges();
            };

            act.Should().NotThrow();
            context.Investment.Should().BeEmpty();
            context.InvestmentModel.Should().BeEmpty();
        }

        [Fact]
        public void TestInvestmentFKExpensesConstraint()
        {
            var investment = new StockInvestment("testAnalysis");
            investment.StockPrice = 100;
            investment.StockQuantity = 10;
            investment.StockTicker = "TST";

            var options = new OptionsDict();

            context.Portfolio.First().Investments.Add(investment);
            context.SaveChanges();

            var testExpense1 = new OneTimeExpense();
            testExpense1.Amount = 100;
            var testExpense2 = new OneTimeExpense();
            testExpense2.Amount = 200;
            context.Investment.First().Expenses.Add(testExpense1);
            context.Investment.First().Expenses.Add(testExpense2);
            context.SaveChanges();

            Action act = () =>
            {
                context.Investment.Remove(investment);
                context.SaveChanges();
            };

            act.Should().NotThrow();
            context.Investment.Should().BeEmpty();
            context.Expense.Should().BeEmpty();
        }

        [Fact]
        public void TestInvestmentFKPortfolioConstraint()
        {
            var investment = new StockInvestment("testAnalysis");
            investment.StockPrice = 100;
            investment.StockQuantity = 10;
            investment.StockTicker = "TST";

            Action act = () =>
            {
                context.Investment.Add(investment);
                context.SaveChanges();
            };

            act.Should().Throw<DbUpdateException>();
        }

        [Fact]
        public void TestInvestmentFKTransfersFromConstraint()
        {
            var investment = new StockInvestment("testAnalysis");
            investment.StockPrice = 100;
            investment.StockQuantity = 10;
            investment.StockTicker = "TST";
            var investment2 = new StockInvestment("testAnalysis");
            investment.StockPrice = 200;
            investment.StockQuantity = 50;
            investment.StockTicker = "TST2";

            context.Portfolio.First().Investments.Add(investment);
            context.Portfolio.First().Investments.Add(investment2);
            context.SaveChanges();

            var transfer = new InvestmentTransfer();
            transfer.SourceInvestment = context.Investment.First(i => i.InvestmentId == 1);
            transfer.DestinationInvestment = context.Investment.First(i => i.InvestmentId == 2);
            context.InvestmentTransfer.Add(transfer);
            context.SaveChanges();

            Action act = () =>
            {
                context.Investment.Remove(investment);
                context.SaveChanges();
            };

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void TestInvestmentFKTransfersConstraint()
        {
            var investment = new StockInvestment("testAnalysis");
            investment.StockPrice = 100;
            investment.StockQuantity = 10;
            investment.StockTicker = "TST";
            var investment2 = new StockInvestment("testAnalysis");
            investment.StockPrice = 200;
            investment.StockQuantity = 50;
            investment.StockTicker = "TST2";

            context.Portfolio.First().Investments.Add(investment);
            context.Portfolio.First().Investments.Add(investment2);
            context.SaveChanges();

            var transfer = new InvestmentTransfer();
            transfer.DestinationInvestment = context.Investment.First(i => i.InvestmentId == 1);
            transfer.SourceInvestment = context.Investment.First(i => i.InvestmentId == 2);
            context.InvestmentTransfer.Add(transfer);
            context.SaveChanges();

            Action act = () =>
            {
                context.Investment.Remove(investment);
                context.SaveChanges();
            };

            act.Should().Throw<InvalidOperationException>();
        }

    }
}
