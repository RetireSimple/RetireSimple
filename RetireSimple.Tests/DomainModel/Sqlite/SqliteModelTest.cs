using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.Services;

using Xunit.Abstractions;

namespace RetireSimple.Tests.DomainModel.Sqlite {


    public class SqliteModelTest : IDisposable {
        InvestmentDBContext context { get; set; }
        private readonly ITestOutputHelper output;

        public SqliteModelTest(ITestOutputHelper output) {
            context = new SqliteInvestmentContext();

            context.Database.Migrate();
            context.Database.EnsureCreated();

            this.output = output;
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
            var options = new Dictionary<string, string>();


            context.Investments.Add(investment);
            context.Investments.Add(investment2);
            context.SaveChanges();
            context.InvestmentModels.Add(investment2.InvokeAnalysis(options));
            context.InvestmentModels.Add(investment.InvokeAnalysis(options));
            context.SaveChanges();

            context.InvestmentModels.Should().HaveCount(2);
        }

        [Fact]
        public void TestInvestmentModelFKConstraintonInvestment() {
            var investment = new StockInvestment("testAnalysis");
            var investment2 = new StockInvestment("testAnalysis2");
            var options = new Dictionary<string, string>();

            context.Investments.Add(investment);
            context.Investments.Add(investment2);
            context.SaveChanges();
            context.InvestmentModels.Add(investment2.InvokeAnalysis(options));
            context.InvestmentModels.Add(investment.InvokeAnalysis(options));
            context.SaveChanges();

            Action act = () => {
                context.Investments.Remove(investment);
            };

            act.Should().Throw<InvalidOperationException>();
        }

    }
}
