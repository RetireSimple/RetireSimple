using RetireSimple.Engine.Api;

namespace RetireSimple.Tests.Api {
	public class InvestmentApiTests : IDisposable {
		private readonly ITestOutputHelper output;
		private readonly InvestmentDBContext context;
		private readonly InvestmentApi api;

		public InvestmentApiTests(ITestOutputHelper _output) {
			output = _output;
			context = new InvestmentDBContext(
				new DbContextOptionsBuilder()
					.UseSqlite("Data Source=InvestmentDB_api_invest.db")
					.Options);
			context.Database.Migrate();
			context.Database.EnsureCreated();

			api = new InvestmentApi(context);
		}

		public void Dispose() {
			context.Database.EnsureDeleted();
			context.Dispose();
		}

		[Fact(Skip = "Not Implemented")]
		public void AddInvestmentUnknownTypeThrowsException() {
			//TODO Implement me
		}


		//TODO Implement the following Tests
		[Theory(Skip = "Not Implemented"),
		InlineData(new object[] { "StockInvestment", typeof(StockInvestment) }),
		InlineData(new object[] { "BondInvestment", typeof(BondInvestment) }),
		InlineData(new object[] { "PensionInvestment", typeof(PensionInvestment) }),
		InlineData(new object[] { "CashInvestment", typeof(CashInvestment) }),]
		public void InvestmentAddStock(string type, Type expectedType) {
			//TODO Implement this test
		}



	}
}
