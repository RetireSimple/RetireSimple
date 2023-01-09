using Microsoft.EntityFrameworkCore;

using RetireSimple.Backend.Services;
using RetireSimple.Backend.DomainModel.Data.InvestmentVehicleBase;

using Xunit.Abstractions;
using RetireSimple.Backend.DomainModel.User;
using RetireSimple.Backend.DomainModel.Data.Investment;

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

            var profile = new Profile();
            profile.Name = "jack";
            profile.Age = 65;
            profile.Status = true;

            var portfolio = new Portfolio();

            context.Profiles.Add(profile);
            context.SaveChanges();
            context.Profiles.First(p => p.ProfileId == 1).Portfolios.Add(portfolio);
            context.SaveChanges();

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
            vehicle.Investments.Add(context.Investments.First(i => i.InvestmentId == 1));
            context.SaveChanges();

            Assert.Single(context.InvestmentVehicles);
        }

        [Fact]
        public void TestInvestmentVehicleRemove() {
            InvestmentVehicleBase vehicle = new Vehicle403b();
            context.Portfolio.First(p => p.PortfolioId == 1).InvestmentVehicles.Add(vehicle);
            context.SaveChanges();
            vehicle.Investments.Add(context.Investments.First(i => i.InvestmentId == 1));
            context.SaveChanges();

            context.InvestmentVehicles.Remove(vehicle);
            context.SaveChanges();

            Assert.Equal(0, context.InvestmentVehicles.Count());
        }

        [Fact]
        public void TestInvestmentVehicleFKConstraintPortfolio() {
            InvestmentVehicleBase vehicle = new Vehicle403b();

            Action act = () => {
                context.InvestmentVehicles.Add(vehicle);
                context.SaveChanges();
            };

            act.Should().Throw<DbUpdateException>();
        }

        [Fact]
        public void TestInvestmentVehicleFKConstraintInvestment() {
            InvestmentVehicleBase vehicle = new Vehicle403b();
            context.Portfolio.First(p => p.PortfolioId == 1).InvestmentVehicles.Add(vehicle);
            context.SaveChanges();
            vehicle.Investments.Add(context.Investments.First(i => i.InvestmentId == 1));
            context.SaveChanges();

            context.InvestmentVehicles.Remove(vehicle);
            context.SaveChanges();

            Assert.Equal(1, context.Investments.Count());
        }

    }
}
