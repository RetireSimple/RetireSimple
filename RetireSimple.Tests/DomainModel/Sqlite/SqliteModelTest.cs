using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.Services;

namespace RetireSimple.Tests.DomainModel.Sqlite {
    public class SqliteModelTest : IDisposable {
        InvestmentDBContext context { get; set; }

        public SqliteModelTest() {
            context = new InvestmentDBContext();

            context.Database.Migrate();
            context.Database.EnsureCreated();
        }

        public void Dispose() {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        //TODO test if data properly serialized later
        [Fact]
        public void TestStockInvestmentAdd() {
            var investment = new StockInvestment("testAnalysis");
            var investment2 = new StockInvestment("testAnalysis2");

            context.Investments.Add(investment);
            context.Investments.Add(investment2);

            context.SaveChanges();

            context.Investments.Should().HaveCount(2);
        }

        [Fact]
        public void TestStockInvestmentModelAdd() {
            var investment = new StockInvestment("testAnalysis");
            var investment2 = new StockInvestment("testAnalysis2");

            context.Investments.Add(investment);
            context.Investments.Add(investment2);
            context.SaveChanges();
            context.InvestmentModels.Add(investment2.InvokeAnalysis());
            context.InvestmentModels.Add(investment.InvokeAnalysis());
            context.SaveChanges();

            context.InvestmentModels.Should().HaveCount(2);
        }

        [Fact]
        public void TestInvestmentModelFKConstraintonInvestment() {
            var investment = new StockInvestment("testAnalysis");
            var investment2 = new StockInvestment("testAnalysis2");

            context.Investments.Add(investment);
            context.Investments.Add(investment2);
            context.SaveChanges();
            context.InvestmentModels.Add(investment2.InvokeAnalysis());
            context.InvestmentModels.Add(investment.InvokeAnalysis());
            context.SaveChanges();

            Action act = () => {
                context.Investments.Remove(investment);
                context.SaveChanges();
            };

            act.Should().Throw<InvalidOperationException>();
        }
    }
}
