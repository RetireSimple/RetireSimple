using RetireSimple.Engine.Api;

namespace RetireSimple.Tests.Api {
	public class InvestmentVehicleApiTests : IDisposable {

		private readonly EngineDbContext context;
		private readonly InvestmentVehicleApi api;

		public InvestmentVehicleApiTests() {
			context = new EngineDbContext(
				new DbContextOptionsBuilder()
					.UseSqlite("Data Source=testing_api_vehicle.db")
					.Options);
			context.Database.Migrate();
			context.Database.EnsureCreated();


			api = new InvestmentVehicleApi(context);
		}

		public void Dispose() {
			context.Database.EnsureDeleted();
			context.Dispose();
		}

		[Theory]
		[InlineData("401k"),
		 InlineData("403b"),
		 InlineData("457"),
		 InlineData("IRA"),
		 InlineData("RothIRA")]
		public void InvestmentVehicleAdd(string type) {
			var result = api.Add(type, "test");

			context.InvestmentVehicle.Should().HaveCount(1);

			var vehicle = context.InvestmentVehicle.First();
			vehicle.InvestmentVehicleName.Should().Be("test");
			vehicle.Investments.Should().BeEmpty();

			result.Should().Be(vehicle.InvestmentVehicleId);
		}

		[Fact]
		public void InvestmentVehicleAddInvestmentThrowsIfNoVehicle() {
			var tempInvestment = new StockInvestment("");
			context.Portfolio.First().Investments.Add(tempInvestment);
			context.SaveChanges();

			Action act = () => {
				api.AddInvestmentToVehicle(1, 1);
			};

			act.Should().Throw<ArgumentException>();
		}

		[Fact]
		public void InvestmentVehicleAddInvestmentThrowsIfNoInvestment() {
			api.Add("401k", "test");

			Action act = () => {
				api.AddInvestmentToVehicle(1, 1);
			};

			act.Should().Throw<ArgumentException>();
		}

		[Theory]
		[InlineData("401k"),
		 InlineData("403b"),
		 InlineData("457"),
		 InlineData("IRA"),
		 InlineData("RothIRA")]
		public void InvestmentVehicleAddInvestmentCreatesRelationship(string type) {
			var vehicleId = api.Add(type, "test");
			var tempInvestment = new StockInvestment("");
			context.Portfolio.First().Investments.Add(tempInvestment);
			context.SaveChanges();

			api.AddInvestmentToVehicle(vehicleId, tempInvestment.InvestmentId);

			var vehicle = context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			vehicle.Investments.Should().ContainSingle();
			context.Investment.Should().ContainSingle();
		}

		[Fact]
		public void InvestmentVehicleRemoveInvestmentThrowsIfNoVehicle() {
			var tempInvestment = new StockInvestment("");
			context.Portfolio.First().Investments.Add(tempInvestment);
			context.SaveChanges();

			Action act = () => {
				api.RemoveInvestmentFromVehicle(1, 1);
			};

			act.Should().Throw<ArgumentException>();
		}

		[Fact]
		public void InvestmentVehicleRemoveInvestmentThrowsIfNoInvestment() {
			api.Add("401k", "test");

			Action act = () => {
				api.RemoveInvestmentFromVehicle(1, 1);
			};

			act.Should().Throw<ArgumentException>();
		}

		[Fact]
		public void InvestmentVehicleRemoveInvestmentThrowsIfInvestmentNotInVehicle() {
			api.Add("401k", "test");
			var tempInvestment = new StockInvestment("");
			context.Portfolio.First().Investments.Add(tempInvestment);
			context.SaveChanges();

			Action act = () => {
				api.RemoveInvestmentFromVehicle(1, 1);
			};

			act.Should().Throw<ArgumentException>();
		}

		[Theory]
		[InlineData("401k"),
		 InlineData("403b"),
		 InlineData("457"),
		 InlineData("IRA"),
		 InlineData("RothIRA")]
		public void InvestmentVehicleRemoveInvestmentDeletesInvestment(string type) {
			var vehicleId = api.Add(type, "test");
			var tempInvestment = new StockInvestment("");
			context.Portfolio.First().Investments.Add(tempInvestment);
			context.SaveChanges();
			api.AddInvestmentToVehicle(vehicleId, tempInvestment.InvestmentId);

			api.RemoveInvestmentFromVehicle(vehicleId, tempInvestment.InvestmentId);

			var vehicle = context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			vehicle.Investments.Should().BeEmpty();
			context.Investment.Should().BeEmpty();
		}

		[Fact]
		public void InvestmentVehicleRemoveThrowsIfNoVehicle() {
			Action act = () => {
				api.Remove(1);
			};

			act.Should().Throw<ArgumentException>();
		}

		[Theory]
		[InlineData("401k"),
		InlineData("403b"),
		InlineData("457"),
		InlineData("IRA"),
		InlineData("RothIRA")]
		public void InvestmentVehicleRemoveCascadeDeletesDependents(string type) {
			var vehicleId = api.Add(type, "test");
			var tempInvestment = new StockInvestment("");
			context.Portfolio.First().Investments.Add(tempInvestment);
			context.SaveChanges();
			api.AddInvestmentToVehicle(vehicleId, tempInvestment.InvestmentId);

			context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId).InvestmentVehicleModel
				= new InvestmentVehicleModel();
			context.SaveChanges();

			api.Remove(vehicleId);

			context.InvestmentVehicle.Should().BeEmpty();
			context.Investment.Should().BeEmpty();
			context.InvestmentVehicleModel.Should().BeEmpty();
		}

		[Fact]
		public void InvestmentVehicleUpdateAnalysisOverridesThrowsIfNoVehicle() {
			Action act = () => {
				api.UpdateAnalysisOverrides(1, new OptionsDict { ["test"] = "test" });
			};

			act.Should().Throw<ArgumentException>();
		}

		[Fact]
		public void InvestmentVehicleUpdateAnalysisOverridesAddsOptionIfNotExist() {
			var vehicleId = api.Add("401k", "test");
			api.UpdateAnalysisOverrides(vehicleId, new OptionsDict { ["test"] = "test" });

			var vehicle = context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			vehicle.AnalysisOptionsOverrides.Should().ContainSingle();
			vehicle.AnalysisOptionsOverrides.ContainsKey("test").Should().BeTrue();
			vehicle.AnalysisOptionsOverrides["test"].Should().Be("test");
		}

		[Fact]
		public void InvestmentVehicleUpdateAnalysisOverridesUpdatesOptionIfExist() {
			var vehicleId = api.Add("401k", "test");
			api.UpdateAnalysisOverrides(vehicleId, new OptionsDict { ["test"] = "test" });
			api.UpdateAnalysisOverrides(vehicleId, new OptionsDict { ["test"] = "test2" });

			var vehicle = context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			vehicle.AnalysisOptionsOverrides.Should().ContainSingle();
			vehicle.AnalysisOptionsOverrides.ContainsKey("test").Should().BeTrue();
			vehicle.AnalysisOptionsOverrides["test"].Should().Be("test2");
		}

		[Fact]
		public void InvestmentVehicleUpdateAnalysisOverridesRemovesOptionIfExistAndNullValue() {
			var vehicleId = api.Add("401k", "test");
			api.UpdateAnalysisOverrides(vehicleId, new OptionsDict { ["test"] = "test" });
			api.UpdateAnalysisOverrides(vehicleId, new OptionsDict { ["test"] = "" });

			var vehicle = context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			vehicle.AnalysisOptionsOverrides.Should().BeEmpty();
			vehicle.AnalysisOptionsOverrides.ContainsKey("test").Should().BeFalse();

		}

		[Fact]
		public void InvestmentVehicleGetVehicleInvestmentsThrowsIfNoVehicle() {
			Action act = () => {
				api.GetVehicleInvestments(1);
			};

			act.Should().Throw<ArgumentException>();
		}

		[Fact]
		public void InvestmentVehicleGetVehicleInvestmentsReturnsInvestmentList() {
			var vehicleId = api.Add("401k", "test");
			var tempInvestment = new StockInvestment("");
			context.Portfolio.First().Investments.Add(tempInvestment);
			context.SaveChanges();
			api.AddInvestmentToVehicle(vehicleId, tempInvestment.InvestmentId);

			var result = api.GetVehicleInvestments(vehicleId);

			result.Should().ContainSingle();
			result.First().Should().Be(tempInvestment);
		}

		[Fact]
		public void InvestmentVehicleGetAllVehicleInvestmentsNoVehiclesReturnsEmptyDictionary() {
			var result = api.GetAllVehicleInvestments();

			result.Should().BeEmpty();
		}


		[Fact]
		public void InvestmentVehicleGetAllVehicleInvestmentsSingleVehicleReturnsDictionaryWithInvestments() {
			var vehicleId = api.Add("401k", "test");
			var tempInvestment = new StockInvestment("");
			context.Portfolio.First().Investments.Add(tempInvestment);
			context.SaveChanges();
			api.AddInvestmentToVehicle(vehicleId, tempInvestment.InvestmentId);

			var result = api.GetAllVehicleInvestments();

			result.Should().ContainSingle();
			result.ContainsKey(vehicleId).Should().BeTrue();
			result[vehicleId].Should().ContainSingle();
			result[vehicleId].First().Should().Be(tempInvestment);
		}

		[Fact]
		public void InvestmentVehicleGetAllVehicleInvestmentsMultipleVehiclesReturnsDictionaryWithInvestments() {
			var vehicleId = api.Add("401k", "test");
			var tempInvestment = new StockInvestment("");
			context.Portfolio.First().Investments.Add(tempInvestment);
			context.SaveChanges();
			api.AddInvestmentToVehicle(vehicleId, tempInvestment.InvestmentId);

			var vehicleId2 = api.Add("401k", "test2");
			var tempInvestment2 = new StockInvestment("");
			context.Portfolio.First().Investments.Add(tempInvestment2);
			context.SaveChanges();
			api.AddInvestmentToVehicle(vehicleId2, tempInvestment2.InvestmentId);

			var result = api.GetAllVehicleInvestments();

			result.Should().HaveCount(2);
			result.ContainsKey(vehicleId).Should().BeTrue();
			result[vehicleId].Should().ContainSingle();
			result[vehicleId].First().Should().Be(tempInvestment);
			result.ContainsKey(vehicleId2).Should().BeTrue();
			result[vehicleId2].Should().ContainSingle();
			result[vehicleId2].First().Should().Be(tempInvestment2);

		}

		[Fact]
		public void InvestmentVehicleUpdateNameThrowsIfNoVehicle() {
			Action act = () => {
				api.UpdateName(1, "test");
			};

			act.Should().Throw<ArgumentException>();
		}

		[Fact]
		public void InvestmentVehicleUpdateNameUpdatesName() {
			var vehicleId = api.Add("401k", "test");
			api.UpdateName(vehicleId, "test2");

			var vehicle = context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			vehicle.InvestmentVehicleName.Should().Be("test2");
		}

		[Fact]
		public void InvestmentVehicleUpdateCashBalanceThrowsIfNoVehicle() {
			Action act = () => {
				api.UpdateCashContributions(1, 100);
			};

			act.Should().Throw<ArgumentException>();
		}

		[Fact]
		public void InvestmentVehicleUpdateCashBalanceUpdatesCashBalance() {
			var vehicleId = api.Add("401k", "test");
			api.UpdateCashContributions(vehicleId, 100);

			var vehicle = context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			vehicle.CashHoldings.Should().Be(100);
		}
	}
}
