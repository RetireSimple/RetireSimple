using Microsoft.EntityFrameworkCore;

using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.DomainModel.User;
using RetireSimple.Backend.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace RetireSimple.Tests.DomainModel.Sqlite {
    public class InvestmentTransfersTests {
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

        //TODO Tests to add
        // 1. InvestmentTransfer Model Add

        [Fact]
        public void TestInvestmentTransferAdd()
        {
            InvestmentTransfer transfer = new InvestmentTransfer();
            transfer.SourceInvestment = context.Investments.First(i => i.InvestmentId == 1);
            transfer.DestinationInvestment = context.Investments.First(i => i.InvestmentId == 2);
            context.Portfolio.First(p => p.PortfolioId == 1).Transfers.Add(transfer);
            context.SaveChanges();

            Assert.Equal(1, context.InvestmentTransfers.Count());

        }
        // 2. InvestmentTransfer Model Remove

        [Fact]
        public void TestInvestmentTransferRemove()
        {
            InvestmentTransfer transfer = new InvestmentTransfer();
            transfer.SourceInvestment = context.Investments.First(i => i.InvestmentId == 1);
            transfer.DestinationInvestment = context.Investments.First(i => i.InvestmentId == 2);
            context.Portfolio.First(p => p.PortfolioId == 1).Transfers.Add(transfer);
            context.SaveChanges();


            context.InvestmentTransfers.Remove(transfer);
            context.SaveChanges();

            Assert.Equal(0, context.InvestmentTransfers.Count());

        }
        // 3. InvestmentTransfer Model FK -> Requires Investments + Portfolio

        [Fact]
        public void TestInvestmentTransferFKConstraint()
        {
            InvestmentTransfer transfer = new InvestmentTransfer();
            //transfer.SourceInvestment = context.Investments.First(i => i.InvestmentId == 1);
            //transfer.DestinationInvestment = context.Investments.First(i => i.InvestmentId == 2);
            //context.Portfolio.First(p => p.PortfolioId == 1).Transfers.Add(transfer);
            Action act = () => {
                context.InvestmentTransfers.Add(transfer);
                context.SaveChanges();
            };

            act.Should().Throw<DbUpdateException>();
        }

    }
}
