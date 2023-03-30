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
				["bondFaceValue"] = "0",
				["bondPurchaseDate"] = DateTime.Now.ToString("yyyy-MM-dd"),
				["bondCurrentPrice"] = "0",
			});
		}

		[Fact]
		public void AddInvestmentBondWithValues() {
			var result = api.Add("BondInvestment", new OptionsDict {
				{"bondTicker", "AAPL"},
				{"bondCouponRate", "0.05"},
				{"bondYieldToMaturity", "0.05"},
				{"bondMaturityDate", "2020-01-01"},
				{"bondFaceValue", "100"},
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
					{"bondFaceValue", "100"},
					{"bondPurchaseDate", "2020-01-01"},
					{"bondCurrentPrice", "100"},
				});
		}

		[Fact]
		public void AddInvestmentBondPartialValues() {
			var result = api.Add("BondInvestment", new OptionsDict {
				{"bondTicker", "AAPL"},
				{"bondCouponRate", "0.05"},
				{"bondYieldToMaturity", "0.05"},
				{"bondMaturityDate", "2020-01-01"},
				{"bondFaceValue", "100"},
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
					{"bondFaceValue", "100"},
					{"bondPurchaseDate", "2020-01-01"},
					{"bondCurrentPrice", "100"},
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

		[Fact]
		public void UpdateInvestmentNotFoundThrows() {
			Action act = () => api.Update(1, new OptionsDict());
			act.Should().Throw<ArgumentException>();
		}

		[Fact]
		public void UpdateInvestmentOnlyExistingValues() {
			api.Add("StockInvestment"); //Using defaults

			api.Update(1, new OptionsDict {
				{"stockTicker", "MSFT"},
			});

			context.Investment.Should().ContainSingle();
			var investment = context.Investment.First();
			investment.InvestmentData["stockTicker"].Should().Be("MSFT");
		}

		[Fact]
		public void UpdateInvestmentHandlesNameSeparately() {
			api.Add("StockInvestment"); //Using defaults

			api.Update(1, new OptionsDict {
				{"stockTicker", "MSFT"},
				{"investmentName", "My Investment"},
			});

			context.Investment.Should().ContainSingle();
			var investment = context.Investment.First();
			investment.InvestmentData["stockTicker"].Should().Be("MSFT");
			investment.InvestmentData.Should().NotContainKey("investmentName");
			investment.InvestmentName.Should().Be("My Investment");
		}

		[Fact]
		public void UpdateInvestmentHandlesAnalysisTypeSeparately() {
			api.Add("StockInvestment"); //Using defaults

			api.Update(1, new OptionsDict {
				{"stockTicker", "MSFT"},
				{"analysisType", "MonteCarlo_LogNormalDist"},
			});

			context.Investment.Should().ContainSingle();
			var investment = context.Investment.First();
			investment.InvestmentData["stockTicker"].Should().Be("MSFT");
			investment.InvestmentData.Should().NotContainKey("analysisType");
			investment.AnalysisType.Should().Be("MonteCarlo_LogNormalDist");
		}

		[Fact]
		public void UpdateInvestmentIgnoresUnknownKeys() {
			api.Add("StockInvestment"); //Using defaults

			api.Update(1, new OptionsDict{
				{"unknownKey", "value"},
				{"stockTicker", "MSFT"},
			});

			context.Investment.Should().ContainSingle();
			var investment = context.Investment.First();
			investment.InvestmentData["stockTicker"].Should().Be("MSFT");
			investment.InvestmentData.Should().NotContainKey("unknownKey");
		}

		[Fact]
		public void UpdateInvestmentChangesLastUpdateField() {
			api.Add("StockInvestment"); //Using defaults

			api.Update(1, new OptionsDict {
				{"stockTicker", "MSFT"},
			});

			context.Investment.Should().ContainSingle();
			var investment = context.Investment.First();
			var prevTime = investment.LastUpdated ?? DateTime.Now;
			investment.LastUpdated.Should().NotBeNull();

			Thread.Sleep(1000); //Wait for a second to ensure LastUpdated changes

			api.Update(1, new OptionsDict {
				{"stockPrice", "123.23"},
			});

			investment.LastUpdated.Should().BeAfter(prevTime);
		}

		[Fact]
		public void UpdateAnalysisOptionInvestmentNotFoundThrows() {
			Action act = () => api.UpdateAnalysisOptions(1, new OptionsDict());
			act.Should().Throw<ArgumentException>();
		}

		[Fact]
		public void UpdateAnalysisOptionAddsNewKeys() {
			api.Add("StockInvestment"); //Using defaults

			api.UpdateAnalysisOptions(1, new OptionsDict {
				{"analysisLength", "60"},
				{"simCount", "10000"},
			});

			var investment = context.Investment.First();
			investment.AnalysisOptionsOverrides["analysisLength"].Should().Be("60");
			investment.AnalysisOptionsOverrides["simCount"].Should().Be("10000");
		}

		[Fact]
		public void UpdateAnalysisOptionRemovesKeysWithEmptyString() {
			api.Add("StockInvestment"); //Using defaults
			var investment = context.Investment.First();
			investment.AnalysisOptionsOverrides["analysisLength"] = "60";
			investment.AnalysisOptionsOverrides["simCount"] = "10000";
			context.SaveChanges();

			api.UpdateAnalysisOptions(1, new OptionsDict {
				{"analysisLength", ""},
			});

			investment.AnalysisOptionsOverrides.Should().NotContainKey("analysisLength");
			investment.AnalysisOptionsOverrides.Should().ContainKey("simCount");
		}

		[Fact]
		public void UpdateAnalysisOptionsUpdatesExistingKeys() {
			api.Add("StockInvestment"); //Using defaults
			var investment = context.Investment.First();
			investment.AnalysisOptionsOverrides["analysisLength"] = "60";
			investment.AnalysisOptionsOverrides["simCount"] = "10000";
			context.SaveChanges();

			api.UpdateAnalysisOptions(1, new OptionsDict {
				{"analysisLength", "120"},
			});

			investment.AnalysisOptionsOverrides["analysisLength"].Should().Be("120");
			investment.AnalysisOptionsOverrides["simCount"].Should().Be("10000");
		}

		[Fact]
		public void UpdateAnalysisOptionsChangesLastUpdateField() {
			api.Add("StockInvestment"); //Using defaults

			api.UpdateAnalysisOptions(1, new OptionsDict {
				{"simCount", "10000"},
			});

			context.Investment.Should().ContainSingle();
			var investment = context.Investment.First();
			var prevTime = investment.LastUpdated ?? DateTime.Now;
			investment.LastUpdated.Should().NotBeNull();

			Thread.Sleep(1000); //Wait for a second to ensure LastUpdated changes

			api.UpdateAnalysisOptions(1, new OptionsDict {
				{"analysisLength", "60"},
			});

			investment.LastUpdated.Should().BeAfter(prevTime);
		}

		[Fact]
		public void GetSingluarInvestmentsNoVehicles() {
			api.Add("StockInvestment");
			api.Add("StockInvestment");
			api.Add("StockInvestment");
			api.Add("StockInvestment");

			var investments = api.GetSingluarInvestments();
			investments.Should().HaveCount(4);
		}

		[Fact]
		public void GetSingularInvestmentsNoInvestments() {
			var investments = api.GetSingluarInvestments();
			investments.Should().BeEmpty();
		}

		[Fact]
		public void GetSingularInvestmentsIgnoresVehicles() {
			api.Add("StockInvestment");
			api.Add("StockInvestment");
			api.Add("StockInvestment");
			api.Add("StockInvestment");

			context.Portfolio.First().InvestmentVehicles.Add(new Vehicle401k());
			context.SaveChanges();
			context.InvestmentVehicle.First().Investments.Add(context.Investment.First());
			context.SaveChanges();

			var investments = api.GetSingluarInvestments();
			investments.Should().HaveCount(3);
			investments.Should().NotContain(i => i.InvestmentId == 1);
		}

		[Fact]
		public void GetAnalysisCurrentModelOnlyReturns() {
			api.Add("StockInvestment");
			var investment = context.Investment.First();
			var invokeTime = DateTime.Now;
			var model = new InvestmentModel() {
				InvestmentId = investment.InvestmentId,
				LastUpdated = invokeTime,
				MinModelData = new List<decimal>() { 1m, 2m, 3m, 4m, 5m },
				AvgModelData = new List<decimal>() { 1m, 2m, 3m, 4m, 5m },
				MaxModelData = new List<decimal>() { 1m, 2m, 3m, 4m, 5m },
			};
			investment.InvestmentModel = model;
			investment.LastUpdated = invokeTime;
			context.SaveChanges();

			var analysis = api.GetAnalysis(investment.InvestmentId);
			analysis.Should().NotBeNull();
			analysis.Should().Be(model);
		}

		//These tests purely checks if the analysis is run, not if the results are correct.
		//This is especially important since we set the AnalysisMethod to null, and validate
		//if an InvalidOperationException is thrown as the call to InvokeAnalysis should throw
		//under these conditions.
		//Analysis validity should be done in separate test
		[Fact]
		public void GetAnalysisOutdatedModelRunsAnalysis() {
			api.Add("StockInvestment");
			if (context.Investment.First() is not StockInvestment investment) {
				throw new ArgumentException("Investment object was unexpectedly null");
			}
			investment.AnalysisMethod = null;
			var invokeTime = DateTime.Now;
			investment.LastUpdated = invokeTime;
			var model = new InvestmentModel() {
				InvestmentId = investment.InvestmentId,
				LastUpdated = invokeTime.AddDays(-1d),
				MinModelData = new List<decimal>() { 1m, 2m, 3m, 4m, 5m },
				AvgModelData = new List<decimal>() { 1m, 2m, 3m, 4m, 5m },
				MaxModelData = new List<decimal>() { 1m, 2m, 3m, 4m, 5m },
			};
			investment.InvestmentModel = model;
			context.SaveChanges();

			Action act = () => api.GetAnalysis(investment.InvestmentId);
			act.Should().Throw<InvalidOperationException>();
		}

		[Fact]
		public void GetAnalysisNoModelRunsAnalysis() {
			api.Add("StockInvestment");
			if (context.Investment.First() is not StockInvestment investment) {
				throw new ArgumentNullException("Investment Object was unexpectedly null");
			}
			context.SaveChanges();

			Action act = () => api.GetAnalysis(investment.InvestmentId);
			act.Should().Throw<InvalidOperationException>();
		}

		[Fact]
		public void GetAnalysisGivenEmptyOptionsDoesNotRunAnalysis() {
			api.Add("StockInvestment");
			if (context.Investment.First() is not StockInvestment investment) {
				throw new ArgumentNullException("Investment Object was unexpectedly null");
			}
			investment.AnalysisMethod = null;
			var invokeTime = DateTime.Now;
			investment.LastUpdated = invokeTime;
			var model = new InvestmentModel() {
				InvestmentId = investment.InvestmentId,
				LastUpdated = invokeTime,
				MinModelData = new List<decimal>() { 1m, 2m, 3m, 4m, 5m },
				AvgModelData = new List<decimal>() { 1m, 2m, 3m, 4m, 5m },
				MaxModelData = new List<decimal>() { 1m, 2m, 3m, 4m, 5m },
			};
			investment.InvestmentModel = model;
			context.SaveChanges();

			Action act = () => api.GetAnalysis(investment.InvestmentId, new OptionsDict());
			act.Should().NotThrow<InvalidOperationException>();

			var result = api.GetAnalysis(investment.InvestmentId, new OptionsDict());
			result.Should().Be(model);
		}

		[Fact]
		public void GetAnalysisGivenOptionsRunsAnalysis() {
			api.Add("StockInvestment");
			if (context.Investment.First() is not StockInvestment investment) {
				throw new ArgumentNullException("Investment Object was unexpectedly null");
			}
			investment.AnalysisMethod = null;
			var invokeTime = DateTime.Now;
			investment.LastUpdated = invokeTime;
			var model = new InvestmentModel() {
				InvestmentId = investment.InvestmentId,
				LastUpdated = invokeTime.AddDays(-1d),
				MinModelData = new List<decimal>() { 1m, 2m, 3m, 4m, 5m },
				AvgModelData = new List<decimal>() { 1m, 2m, 3m, 4m, 5m },
				MaxModelData = new List<decimal>() { 1m, 2m, 3m, 4m, 5m },
			};
			investment.InvestmentModel = model;
			context.SaveChanges();

			Action act = () => api.GetAnalysis(investment.InvestmentId, new OptionsDict(){
				{ "test", "test" }
			});
			act.Should().Throw<InvalidOperationException>();
		}

		//Regression Test: #183
		[Fact]
		public void CreateStockChecksAnalysisTypeSetsAssignedType() {
			//This is intentionally a different option, as we are checking if the field is
			//actually set to the correct value.
			var paramsDict = new OptionsDict() {
				{ "analysisType", "MonteCarlo_LogNormalDist" }
			};

			var stock = InvestmentApiUtil.CreateStock(paramsDict);

			stock.AnalysisType.Should().Be("MonteCarlo_LogNormalDist");
		}

		//Regression Test: #183
		[Fact]
		public void CreateStockFieldNotFoundSetsDefaultType() {
			var stock = InvestmentApiUtil.CreateStock(new OptionsDict());

			stock.AnalysisType.Should().Be("MonteCarlo_NormalDist");
		}

		//Regression Test: #183
		[Fact]
		public void CreateBondChecksAnalysisTypeSetsAssignedType() {
			//This is intentionally a different option, as we are checking if the field is
			//actually set to the correct value.
			var paramsDict = new OptionsDict() {
				{ "analysisType", "StdBondValuation" }
			};

			var bond = InvestmentApiUtil.CreateBond(paramsDict);

			bond.AnalysisType.Should().Be("StdBondValuation");
		}

		//Regression Test: #183
		[Fact]
		public void CreateBondFieldNotFoundSetsDefaultType() {
			var bond = InvestmentApiUtil.CreateBond(new OptionsDict());

			bond.AnalysisType.Should().Be("StdBondValuation");
		}
	}
}

