using RetireSimple.Engine.Api;

namespace RetireSimple.Tests.Api {
	public class InvestmentApiTests : IDisposable {
		private readonly EngineDbContext context;
		private readonly InvestmentApi api;

		public InvestmentApiTests() {
			context = new EngineDbContext(
				new DbContextOptionsBuilder()
					.UseSqlite("Data Source=testing_api_invest.db")
					.Options);
			context.Database.Migrate();
			context.Database.EnsureCreated();

			api = new InvestmentApi(context);
		}

		public void Dispose() {
			context.Database.EnsureDeleted();
			context.Dispose();
		}

		[Fact]
		public void AddInvestmentUnknownTypeThrowsException() {
			Action act = () => { api.Add("test"); };

			act.Should().Throw<ArgumentException>();
		}

		[Fact]
		public void AddInvestmentStockAllDefaults() {
			var result = api.Add("StockInvestment");

			context.Investment.Should().HaveCount(1);

			var investment = context.Investment.First();
			investment.InvestmentType.Should().Be("StockInvestment");
			investment.InvestmentName.Should().Be("");
			investment.InvestmentId.Should().Be(result);
			investment.InvestmentData.Should().BeEquivalentTo(
				new OptionsDict {
					{"stockTicker", "N/A"},
					{"stockQuantity", "0"},
					{"stockPrice", "0"},
					{"stockPurchaseDate", DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd")},
					{"stockDividendPercent", "0"},
					{"stockDividendDistributionInterval", "Month"},
					{"stockDividendDistributionMethod", "Stock"},
					{"stockDividendFirstPaymentDate",DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd")}
				});
		}

		[Fact]
		public void AddInvestmentStockWithValues() {
			var result = api.Add("StockInvestment", new OptionsDict {
				{"stockTicker", "AAPL"},
				{"stockQuantity", "10"},
				{"stockPrice", "100"},
				{"stockPurchaseDate", "2020-01-01"},
				{"stockDividendPercent", "0.5"},
				{"stockDividendDistributionInterval", "Quarter"},
				{"stockDividendDistributionMethod", "Cash"},
				{"stockDividendFirstPaymentDate", "2020-01-01"}
			});

			context.Investment.Should().HaveCount(1);

			var investment = context.Investment.First();
			investment.InvestmentType.Should().Be("StockInvestment");
			investment.InvestmentName.Should().Be("");
			investment.InvestmentId.Should().Be(result);
			investment.InvestmentData.Should().BeEquivalentTo(
				new OptionsDict {
					{"stockTicker", "AAPL"},
					{"stockQuantity", "10"},
					{"stockPrice", "100"},
					{"stockPurchaseDate", "2020-01-01"},
					{"stockDividendPercent", "0.5"},
					{"stockDividendDistributionInterval", "Quarter"},
					{"stockDividendDistributionMethod", "Cash"},
					{"stockDividendFirstPaymentDate", "2020-01-01"}
				});
		}

		[Fact]
		public void AddInvestmentStockWithValuesAndName() {
			var result = api.Add("StockInvestment", new OptionsDict {
				{"investmentName", "Apple Stock"},
				{"stockTicker", "AAPL"},
				{"stockQuantity", "10"},
				{"stockPrice", "100"},
				{"stockPurchaseDate", "2020-01-01"},
				{"stockDividendPercent", "0.5"},
				{"stockDividendDistributionInterval", "Quarter"},
				{"stockDividendDistributionMethod", "Cash"},
				{"stockDividendFirstPaymentDate", "2020-01-01"}
			});

			context.Investment.Should().HaveCount(1);

			var investment = context.Investment.First();
			investment.InvestmentType.Should().Be("StockInvestment");
			investment.InvestmentName.Should().Be("Apple Stock");
			investment.InvestmentId.Should().Be(result);
			investment.InvestmentData.Should().BeEquivalentTo(
				new OptionsDict {
					{"stockTicker", "AAPL"},
					{"stockQuantity", "10"},
					{"stockPrice", "100"},
					{"stockPurchaseDate", "2020-01-01"},
					{"stockDividendPercent", "0.5"},
					{"stockDividendDistributionInterval", "Quarter"},
					{"stockDividendDistributionMethod", "Cash"},
					{"stockDividendFirstPaymentDate", "2020-01-01"}
				});
		}

		[Fact]
		public void AddInvestmentStockPartialValues() {
			var result = api.Add("StockInvestment", new OptionsDict {
				{"stockTicker", "AAPL"},
				{"stockQuantity", "10"},
				{"stockPrice", "100"},
				{"stockPurchaseDate", "2020-01-01"},
				{"stockDividendPercent", "0.5"},
				{"stockDividendDistributionInterval", "Quarter"},
				{"stockDividendDistributionMethod", "Cash"}
			});

			context.Investment.Should().HaveCount(1);

			var investment = context.Investment.First();
			investment.InvestmentType.Should().Be("StockInvestment");
			investment.InvestmentName.Should().Be("");
			investment.InvestmentId.Should().Be(result);
			investment.InvestmentData.Should().BeEquivalentTo(
				new OptionsDict {
					{"stockTicker", "AAPL"},
					{"stockQuantity", "10"},
					{"stockPrice", "100"},
					{"stockPurchaseDate", "2020-01-01"},
					{"stockDividendPercent", "0.5"},
					{"stockDividendDistributionInterval", "Quarter"},
					{"stockDividendDistributionMethod", "Cash"},
					{"stockDividendFirstPaymentDate", DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd")}
				});
		}

		[Fact]
		public void AddInvestmentBondAllDefaults() {
			var result = api.Add("BondInvestment");

			context.Investment.Should().HaveCount(1);

			var investment = context.Investment.First();
			investment.InvestmentType.Should().Be("BondInvestment");
			investment.InvestmentName.Should().Be("");
			investment.InvestmentId.Should().Be(result);
			investment.InvestmentData.Should().BeEquivalentTo(
			new OptionsDict() {
				["bondTicker"] = "N/A",
				["bondCouponRate"] = "0.05",
				["bondYieldToMaturity"] = "0.05",
				["bondMaturityDate"] = DateTime.Now.ToString("yyyy-MM-dd"),
				["bondPurchasePrice"] = "0",
				["bondPurchaseDate"] = DateTime.Now.ToString("yyyy-MM-dd"),
				["bondCurrentPrice"] = "0",
				["bondIsAnnual"] = "true",
			});
		}

		[Fact]
		public void AddInvestmentBondWithValues() {
			var result = api.Add("BondInvestment", new OptionsDict {
				{"bondTicker", "AAPL"},
				{"bondCouponRate", "0.05"},
				{"bondYieldToMaturity", "0.05"},
				{"bondMaturityDate", "2020-01-01"},
				{"bondPurchasePrice", "100"},
				{"bondPurchaseDate", "2020-01-01"},
				{"bondCurrentPrice", "100"},
				{"bondIsAnnual", "false"},
			});

			context.Investment.Should().HaveCount(1);

			var investment = context.Investment.First();
			investment.InvestmentType.Should().Be("BondInvestment");
			investment.InvestmentName.Should().Be("");
			investment.InvestmentId.Should().Be(result);
			investment.InvestmentData.Should().BeEquivalentTo(
				new OptionsDict {
					{"bondTicker", "AAPL"},
					{"bondCouponRate", "0.05"},
					{"bondYieldToMaturity", "0.05"},
					{"bondMaturityDate", "2020-01-01"},
					{"bondPurchasePrice", "100"},
					{"bondPurchaseDate", "2020-01-01"},
					{"bondCurrentPrice", "100"},
					{"bondIsAnnual", "false"},
				});
		}

		[Fact]
		public void AddInvestmentBondPartialValues() {
			var result = api.Add("BondInvestment", new OptionsDict {
				{"bondTicker", "AAPL"},
				{"bondCouponRate", "0.05"},
				{"bondYieldToMaturity", "0.05"},
				{"bondMaturityDate", "2020-01-01"},
				{"bondPurchasePrice", "100"},
				{"bondPurchaseDate", "2020-01-01"},
				{"bondCurrentPrice", "100"},
			});

			context.Investment.Should().HaveCount(1);

			var investment = context.Investment.First();
			investment.InvestmentType.Should().Be("BondInvestment");
			investment.InvestmentName.Should().Be("");
			investment.InvestmentId.Should().Be(result);
			investment.InvestmentData.Should().BeEquivalentTo(
				new OptionsDict {
					{"bondTicker", "AAPL"},
					{"bondCouponRate", "0.05"},
					{"bondYieldToMaturity", "0.05"},
					{"bondMaturityDate", "2020-01-01"},
					{"bondPurchasePrice", "100"},
					{"bondPurchaseDate", "2020-01-01"},
					{"bondCurrentPrice", "100"},
					{"bondIsAnnual", "true"},
				});
		}

		[Fact]
		public void RemoveInvestmentNotFoundThrows() {
			Action act = () => api.Remove(1);
			act.Should().Throw<ArgumentException>();
		}

		[Fact]
		public void RemoveInvestment() {
			context.Portfolio.First().Investments.Add(new StockInvestment(""));
			context.Portfolio.First().Investments.Add(new StockInvestment(""));
			context.SaveChanges();

			api.Remove(1);

			context.Investment.Should().ContainSingle();
		}

		[Fact]
		public void RemoveInvestmentAllowsSpecificCascade() {
			context.Portfolio.First().Investments.Add(new StockInvestment(""));
			context.Portfolio.First().Investments.Add(new StockInvestment(""));
			context.SaveChanges();

			context.Investment.First().InvestmentModel = new InvestmentModel();
			context.SaveChanges();

			context.Expense.Add(new OneTimeExpense {
				SourceInvestmentId = 1,
			});
			context.SaveChanges();


			api.Remove(1);

			context.Investment.Should().ContainSingle();
			context.Expense.Should().BeEmpty();
			context.InvestmentModel.Should().BeEmpty();
		}

		[Fact]
		public void RemoveInvestmentIsTransferSourceThrows() {
			context.Portfolio.First().Investments.Add(new StockInvestment(""));
			context.Portfolio.First().Investments.Add(new StockInvestment(""));
			context.SaveChanges();

			context.InvestmentTransfer.Add(new InvestmentTransfer {
				SourceInvestmentId = 1,
				DestinationInvestmentId = 2,
			});

			Action act = () => api.Remove(1);
			act.Should().Throw<InvalidOperationException>();
		}

		[Fact]
		public void RemoveInvestmentIsTransferDestinationThrows() {
			context.Portfolio.First().Investments.Add(new StockInvestment(""));
			context.Portfolio.First().Investments.Add(new StockInvestment(""));
			context.SaveChanges();

			context.InvestmentTransfer.Add(new InvestmentTransfer {
				SourceInvestmentId = 2,
				DestinationInvestmentId = 1,
			});

			Action act = () => api.Remove(1);
			act.Should().Throw<InvalidOperationException>();
		}
	}

}

