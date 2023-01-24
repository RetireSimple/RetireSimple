namespace RetireSimple.Tests.DomainModel.Sqlite {
    public class InvestmentVehicleTests : IDisposable {
        InvestmentDBContext context { get; set; }

        private readonly ITestOutputHelper output;

        public InvestmentVehicleTests(ITestOutputHelper output) {
            context = new InvestmentDBContext(
                new DbContextOptionsBuilder()
                    .UseSqlite("Data Source=InvestmentDB_vehicletests.db")
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
        public void TestInvestmentVehicleAdd() {
            InvestmentVehicleBase vehicle = new Vehicle403b();
            context.Portfolio.First(p => p.PortfolioId == 1).InvestmentVehicles.Add(vehicle);
            context.SaveChanges();
            vehicle.Investments.Add(context.Investment.First(i => i.InvestmentId == 1));
            context.SaveChanges();

            Assert.Single(context.InvestmentVehicle);
        }

        [Fact]
        public void TestInvestmentVehicleRemove() {
            InvestmentVehicleBase vehicle = new Vehicle403b();
            context.Portfolio.First(p => p.PortfolioId == 1).InvestmentVehicles.Add(vehicle);
            context.SaveChanges();
            vehicle.Investments.Add(context.Investment.First(i => i.InvestmentId == 1));
            context.SaveChanges();

            context.InvestmentVehicle.Remove(vehicle);
            context.SaveChanges();

            Assert.Equal(0, context.InvestmentVehicle.Count());
        }

        [Fact]
        public void TestInvestmentVehicleFKConstraintPortfolio() {
            InvestmentVehicleBase vehicle = new Vehicle403b();

            Action act = () => {
                context.InvestmentVehicle.Add(vehicle);
                context.SaveChanges();
            };

            act.Should().Throw<DbUpdateException>();
        }

        [Fact]
        public void TestInvestmentVehicleFKConstraintInvestmentDeleteCascades() {
            InvestmentVehicleBase vehicle = new Vehicle403b();
            context.Portfolio.First(p => p.PortfolioId == 1).InvestmentVehicles.Add(vehicle);
            vehicle.Investments.Add(context.Investment.First(i => i.InvestmentId == 1));
            context.SaveChanges();

            Action act = () => {
                context.InvestmentVehicle.Remove(vehicle);
                context.SaveChanges();
            };

            act.Should().NotThrow();
            context.Investment.Should().BeEmpty();
            context.InvestmentVehicle.Should().BeEmpty();
        }

        //TODO add cascade test for Vehicle models

    }
}
