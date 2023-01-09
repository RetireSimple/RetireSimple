using Microsoft.EntityFrameworkCore;

using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Expense;
using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.DomainModel.User;
using RetireSimple.Backend.Services;

using Xunit.Abstractions;

namespace RetireSimple.Tests.DomainModel.Sqlite {
    public class PortfolioTests : IDisposable {
        InvestmentDBContext context { get; set; }

        private readonly ITestOutputHelper output;

        public PortfolioTests(ITestOutputHelper output) {
            context = new InvestmentDBContext(
                new DbContextOptionsBuilder()
                    .UseSqlite("Data Source=InvestmentDB_portfoliotests.db")
                    .Options);
            context.Database.Migrate();
            context.Database.EnsureCreated();

            this.output = output;

            //Portfolio Specific Setup
            var profile = new Profile();

            profile.Name = "jack";
            profile.Age = 65;
            profile.Status = true;


            context.Profiles.Add(profile);
            context.SaveChanges();
        }

        public void Dispose() {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void TestPortfolioFKConstraintDelete() {
            var portfolio = new Portfolio();
            context.Profiles.First(p => p.ProfileId == 1).Portfolios.Add(portfolio);

            var investment = new StockInvestment("testAnalysis");
            investment.StockPrice = 100;
            investment.StockQuantity = 10;
            investment.StockTicker = "TST";

            portfolio.Investments.Add(investment);
            context.SaveChanges();

            Assert.Single(context.Investments);


        }

        [Fact]
        public void TestPortfolioFKConstraintProfile() {
            var portfolio = new Portfolio();
            Action act = () => {
                context.Portfolio.Add(portfolio);
                context.SaveChanges();
            };

            act.Should().Throw<DbUpdateException>();
        }

        [Fact]
        public void TestPortfolioAdd() {
            var portfolio = new Portfolio();
            var portfolio2 = new Portfolio();

            context.Profiles.First(p => p.ProfileId == 1).Portfolios.Add(portfolio);
            context.Profiles.First(p => p.ProfileId == 1).Portfolios.Add(portfolio2);

            context.SaveChanges();
            context.Portfolio.Should().HaveCount(2);
        }

        [Fact]
        public void TestPortfolioRemove() {
            var portfolio = new Portfolio();
            var portfolio2 = new Portfolio();

            context.Profiles.First(p => p.ProfileId == 1).Portfolios.Add(portfolio);
            context.Profiles.First(p => p.ProfileId == 1).Portfolios.Add(portfolio2);
            context.SaveChanges();
            context.Portfolio.Remove(portfolio);
            context.SaveChanges();

            context.Portfolio.Should().HaveCount(1);
        }
    }
}
