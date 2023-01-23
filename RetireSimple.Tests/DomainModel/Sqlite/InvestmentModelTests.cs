namespace RetireSimple.Tests.DomainModel.Sqlite {
    public class InvestmentModelTests : IDisposable {
        InvestmentDBContext context { get; set; }

        private readonly ITestOutputHelper output;

        public InvestmentModelTests(ITestOutputHelper output) {
            context = new InvestmentDBContext(
                new DbContextOptionsBuilder()
                    .UseSqlite("Data Source=InvestmentDB_modeltests.db")
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
        public void TestInvestmentModelAdd() {
            InvestmentModel model = new InvestmentModel();
            model.Investment = context.Portfolio.First(p => p.PortfolioId == 1).Investments.First(i => i.InvestmentId == 1);

            context.InvestmentModel.Add(model);
            context.SaveChanges();

            Assert.Single(context.InvestmentModel);
        }

        [Fact]
        public void TestInvestmentModelRemove() {
            InvestmentModel model = new InvestmentModel();
            model.Investment = context.Portfolio.First(p => p.PortfolioId == 1).Investments.First(i => i.InvestmentId == 1);

            context.InvestmentModel.Add(model);
            context.SaveChanges();

            context.InvestmentModel.Remove(model);
            context.SaveChanges();

            Assert.Equal(0, context.InvestmentModel.Count());
        }

        [Fact]
        public void TestInvestmentModelFKConstraint() {
            InvestmentModel model = new InvestmentModel();

            Action act = () => {
                context.InvestmentModel.Add(model);
                context.SaveChanges();
            };

            act.Should().Throw<System.InvalidOperationException>();
        }
    }
}
