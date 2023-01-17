using Microsoft.EntityFrameworkCore;

using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.InvestmentVehicle;
using RetireSimple.Backend.DomainModel.User;
using RetireSimple.Backend.Services;

using Xunit.Abstractions;

namespace RetireSimple.Tests.DomainModel.Sqlite {
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

            var profile = new Profile();
            profile.Name = "jack";
            profile.Age = 65;
            profile.Status = true;

            var portfolio = new Portfolio();
            profile.Portfolios.Add(portfolio);
            context.Profile.Add(profile);
            context.SaveChanges();

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
