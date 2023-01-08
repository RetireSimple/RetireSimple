using Microsoft.EntityFrameworkCore;

using RetireSimple.Backend.Services;

using Xunit.Abstractions;

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
