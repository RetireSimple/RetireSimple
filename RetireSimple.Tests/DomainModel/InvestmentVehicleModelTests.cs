namespace RetireSimple.Tests.DomainModel {
    public class InvestmentVehicleModelTests : IDisposable {
        InvestmentDBContext context { get; set; }

        private readonly ITestOutputHelper output;

        public InvestmentVehicleModelTests(ITestOutputHelper output) {
            context = new InvestmentDBContext(
                new DbContextOptionsBuilder()
                    .UseSqlite("Data Source=InvestmentDB_vmodel.db")
                    .Options);
            context.Database.Migrate();
            //context.Database.EnsureCreated();

            this.output = output;

            var vehicle = new Vehicle401k();
            context.Portfolio.First(p => p.PortfolioId == 1).InvestmentVehicles.Add(vehicle);
            context.SaveChanges();
        }

        public void Dispose() {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void TestInvestmentVehicleModelAdd() {
            InvestmentVehicleModel model = new InvestmentVehicleModel();
            context.Portfolio.First(p => p.PortfolioId == 1)
                .InvestmentVehicles.First(i => i.InvestmentVehicleId == 1)
                .InvestmentVehicleModel = model;

            context.SaveChanges();

            Assert.Single(context.InvestmentVehicleModel);
        }

        [Fact]
        public void TestInvestmentModelRemove() {
            InvestmentVehicleModel model = new InvestmentVehicleModel();
            context.Portfolio.First(p => p.PortfolioId == 1)
                .InvestmentVehicles.First(i => i.InvestmentVehicleId == 1)
                .InvestmentVehicleModel = model;
            context.SaveChanges();


            context.InvestmentVehicleModel.Remove(context.InvestmentVehicleModel.First());
            context.SaveChanges();

            Assert.Empty(context.InvestmentVehicleModel);
        }

        [Fact]
        public void TestInvestmentModelFKConstraint() {
            InvestmentVehicleModel model = new InvestmentVehicleModel();

            Action act = () => {
                context.InvestmentVehicleModel.Add(model);
                context.SaveChanges();
            };

            act.Should().Throw<DbUpdateException>();
        }
    }
}
