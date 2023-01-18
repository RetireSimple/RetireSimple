using Microsoft.EntityFrameworkCore;

using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.DomainModel.User;
using RetireSimple.Backend.Services;

using Xunit.Abstractions;
namespace RetireSimple.Tests.DomainModel.Sqlite {
    public class InvestmentTransfersTests : IDisposable {
        InvestmentDBContext context { get; set; }

        private readonly ITestOutputHelper output;

        public InvestmentTransfersTests(ITestOutputHelper output) {
            context = new InvestmentDBContext(
                new DbContextOptionsBuilder()
                    .UseSqlite("Data Source=InvestmentDB_transferstests.db")
                    .Options);
            context.Database.Migrate();
            context.Database.EnsureCreated();

            this.output = output;

            var profile = new Profile();
            profile.Name = "jack";
            profile.Age = 65;
            profile.Status = true;

            var portfolio = new Portfolio();

            context.Profile.Add(profile);
            context.SaveChanges();
            context.Profile.First(p => p.ProfileId == 1).Portfolios.Add(portfolio);
            context.SaveChanges();

            var investment = new StockInvestment("test");
            investment.StockPrice = 100;
            investment.StockQuantity = 10;
            investment.StockTicker = "TST";
            context.Portfolio.First(p => p.PortfolioId == 1).Investments.Add(investment);
            context.SaveChanges();
            var investment2 = new StockInvestment("test2");
            investment.StockPrice = 100;
            investment.StockQuantity = 10;
            investment.StockTicker = "TST";
            context.Portfolio.First(p => p.PortfolioId == 1).Investments.Add(investment2);
            context.SaveChanges();
        }

        public void Dispose() {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void TestInvestmentTransferAdd() {
            InvestmentTransfer transfer = new InvestmentTransfer();
            transfer.SourceInvestment = context.Investment.First(i => i.InvestmentId == 1);
            transfer.DestinationInvestment = context.Investment.First(i => i.InvestmentId == 2);
            context.InvestmentTransfer.Add(transfer);
            context.SaveChanges();

            Assert.Single(context.InvestmentTransfer);
        }

        [Fact]
        public void TestInvestmentTransferRemove() {
            InvestmentTransfer transfer = new InvestmentTransfer();
            transfer.SourceInvestment = context.Investment.First(i => i.InvestmentId == 1);
            transfer.DestinationInvestment = context.Investment.First(i => i.InvestmentId == 2);
            context.InvestmentTransfer.Add(transfer);
            context.SaveChanges();

            context.InvestmentTransfer.Remove(transfer);
            context.SaveChanges();

            Assert.Empty(context.InvestmentTransfer);
        }

        [Fact]
        public void TestInvestmentTransferFKConstraint() {
            InvestmentTransfer transfer = new InvestmentTransfer();
            Action act = () => {
                context.InvestmentTransfer.Add(transfer);
                context.SaveChanges();
            };

            act.Should().Throw<DbUpdateException>();
        }

    }
}
