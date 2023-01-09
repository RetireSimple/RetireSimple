using Microsoft.EntityFrameworkCore;

using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.DomainModel.User;
using RetireSimple.Backend.Services;

using Xunit.Abstractions;

namespace RetireSimple.Tests.DomainModel.Sqlite {
    public class InvestmentTests : IDisposable {
        InvestmentDBContext context { get; set; }

        private readonly ITestOutputHelper output;

        public InvestmentTests(ITestOutputHelper output) {
            context = new InvestmentDBContext(
                new DbContextOptionsBuilder()
                    .UseSqlite("Data Source=InvestmentDB_invtests.db")
                    .Options);
            context.Database.Migrate();
            context.Database.EnsureCreated();

            this.output = output;

            //Investment Specific Setup

            var profile = new Profile();
            profile.Name = "jack";
            profile.Age = 65;
            profile.Status = true;

            var portfolio = new Portfolio();

            context.Profiles.Add(profile);
            context.SaveChanges();
            context.Profiles.First(p => p.ProfileId == 1).Portfolios.Add(portfolio);
            context.SaveChanges();
        }

        public void Dispose() {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void TestStockInvestmentAdd() {
            var investment = new StockInvestment("testAnalysis");
            investment.StockPrice = 100;
            investment.StockQuantity = 10;
            investment.StockTicker = "TST";

            context.Portfolio.First(p => p.PortfolioId == 1).Investments.Add(investment);
            context.SaveChanges();

            Assert.Equal(1, context.Investments.Count());
            Assert.Single(context.Portfolio.First(p => p.PortfolioId == 1).Investments);
            Assert.Single(context.Profiles.First(p => p.ProfileId == 1).Portfolios.First().Investments);
        }

        [Fact]
        public void TestStockInvestmentRemove() {
            var investment = new StockInvestment("testAnalysis");
            investment.StockPrice = 100;
            investment.StockQuantity = 10;
            investment.StockTicker = "TST";

            context.Portfolio.First(p => p.PortfolioId == 1).Investments.Add(investment);
            context.SaveChanges();

            context.Portfolio.First(p => p.PortfolioId == 1).Investments.Remove(investment);
            context.SaveChanges();

            Assert.Equal(0, context.Investments.Count());
            Assert.Empty(context.Portfolio.First(p => p.PortfolioId == 1).Investments);
        }

        [Fact]
        public void TestInvestmentFKInvestmentModelConstraint() {
            var investment = new StockInvestment("testAnalysis");
            investment.StockPrice = 100;
            investment.StockQuantity = 10;
            investment.StockTicker = "TST";

            var options = new Dictionary<string, string>();

            context.Portfolio.First().Investments.Add(investment);
            context.SaveChanges();

            context.InvestmentModels.Add(investment.InvokeAnalysis(options));

            context.SaveChanges();
            Action act = () => {
                context.Investments.Remove(investment);
            };

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void TestInvestmentFKPortfolioModelConstraint() {
            //TODO
        }

        [Fact]
        public void TestInvestmentFKInvestmentVehicleNotRequired() {
            //TODO
        }

    }
}
