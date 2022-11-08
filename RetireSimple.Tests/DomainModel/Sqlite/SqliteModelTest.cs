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
            context = new InvestmentDBContext(
                new DbContextOptionsBuilder()
                    .UseSqlite("Data Source=InvestmentDB.db")
                    //x => x.MigrationsAssembly("RetireSimple.Migrations.Sqlite"))
                    .Options);
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
            investment.StockPrice = 100;
            investment.StockQuantity = 10;
            investment.StockTicker = "TST";

            var investment2 = new StockInvestment("testAnalysis2");
            investment2.StockPrice = 200;
            investment2.StockQuantity = 20;
            investment2.StockTicker = "TEST";

            context.Investments.Add(investment);
            context.Investments.Add(investment2);

            context.SaveChanges();

            context.Investments.Should().HaveCount(2);
        }

        [Fact]
        public void TestStockInvestmentModelAdd() {
            var investment = new StockInvestment("testAnalysis");
            investment.StockPrice = 100;
            investment.StockQuantity = 10;
            investment.StockTicker = "TST";

            var investment2 = new StockInvestment("testAnalysis2");
            investment2.StockPrice = 200;
            investment2.StockQuantity = 20;
            investment2.StockTicker = "TEST";

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
            investment.StockPrice = 100;
            investment.StockQuantity = 10;
            investment.StockTicker = "TST";

            var investment2 = new StockInvestment("testAnalysis2");
            investment2.StockPrice = 200;
            investment2.StockQuantity = 20;
            investment2.StockTicker = "TEST";

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
