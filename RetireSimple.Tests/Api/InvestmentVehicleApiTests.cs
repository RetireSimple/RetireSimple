using RetireSimple.Engine.Api;
using RetireSimple.Engine.Data.Analysis;

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
		[InlineData("Vehicle401k"),
		 InlineData("Vehicle403b"),
		 InlineData("Vehicle457"),
		 InlineData("VehicleIRA"),
		 InlineData("VehicleRothIRA")]
		public void InvestmentVehicleAdd(string type) {
			var result = api.Add(type, new OptionsDict { ["investmentVehicleName"] = "test", ["cashHoldings"] = "10" });

			context.InvestmentVehicle.Should().HaveCount(1);

			var vehicle = context.InvestmentVehicle.First();
			vehicle.InvestmentVehicleName.Should().Be("test");
			vehicle.CashHoldings.Should().Be(10);
			vehicle.Investments.Should().BeEmpty();

			result.Should().Be(vehicle.InvestmentVehicleId);
		}

		[Theory]
		[InlineData("Vehicle401k"),
		 InlineData("Vehicle403b"),
		 InlineData("Vehicle457"),
		 InlineData("VehicleIRA"),
		 InlineData("VehicleRothIRA")]
		public void InvestmentVehicleAddWithNoCashSpecifiedUsesZero(string type) {
			var result = api.Add(type, new OptionsDict { ["investmentVehicleName"] = "test" });

			context.InvestmentVehicle.Should().HaveCount(1);

			var vehicle = context.InvestmentVehicle.First();
			vehicle.InvestmentVehicleName.Should().Be("test");
			vehicle.CashHoldings.Should().Be(0);
			vehicle.Investments.Should().BeEmpty();

			result.Should().Be(vehicle.InvestmentVehicleId);
		}

		[Theory]
		[InlineData("Vehicle401k"),
		 InlineData("Vehicle403b"),
		 InlineData("Vehicle457"),
		 InlineData("VehicleIRA"),
		 InlineData("VehicleRothIRA")]
		public void InvestmentVehicleAddIgnoresInvalidKeys(string type) {
			var result = api.Add(type, new OptionsDict { ["investmentVehicleName"] = "test", ["cashHoldings"] = "10", ["invalid"] = "test" });

			context.InvestmentVehicle.Should().HaveCount(1);

			var vehicle = context.InvestmentVehicle.First();
			vehicle.InvestmentVehicleName.Should().Be("test");
			vehicle.CashHoldings.Should().Be(10);
			vehicle.Investments.Should().BeEmpty();
			vehicle.InvestmentVehicleData.Should().NotContainKey("invalid");
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
			api.Add("Vehicle401k", new OptionsDict { ["investmentVehicleName"] = "test" });

			Action act = () => {
				api.AddInvestmentToVehicle(1, 1);
			};

			act.Should().Throw<ArgumentException>();
		}

		[Theory]
		[InlineData("Vehicle401k"),
		 InlineData("Vehicle403b"),
		 InlineData("Vehicle457"),
		 InlineData("VehicleIRA"),
		 InlineData("VehicleRothIRA")]
		public void InvestmentVehicleAddInvestmentCreatesRelationship(string type) {
			var vehicleId = api.Add(type, new OptionsDict { ["investmentVehicleName"] = "test" });
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
			api.Add("Vehicle401k", new OptionsDict { ["investmentVehicleName"] = "test" });

			Action act = () => {
				api.RemoveInvestmentFromVehicle(1, 1);
			};

			act.Should().Throw<ArgumentException>();
		}

		[Fact]
		public void InvestmentVehicleRemoveInvestmentThrowsIfInvestmentNotInVehicle() {
			api.Add("Vehicle401k", new OptionsDict { ["investmentVehicleName"] = "test" });
			var tempInvestment = new StockInvestment("");
			context.Portfolio.First().Investments.Add(tempInvestment);
			context.SaveChanges();

			Action act = () => {
				api.RemoveInvestmentFromVehicle(1, 1);
			};

			act.Should().Throw<ArgumentException>();
		}

		[Theory]
		[InlineData("Vehicle401k"),
		 InlineData("Vehicle403b"),
		 InlineData("Vehicle457"),
		 InlineData("VehicleIRA"),
		 InlineData("VehicleRothIRA")]
		public void InvestmentVehicleRemoveInvestmentDeletesInvestment(string type) {
			var vehicleId = api.Add(type, new OptionsDict { ["investmentVehicleName"] = "test" });
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
		[InlineData("Vehicle401k"),
		InlineData("Vehicle403b"),
		InlineData("Vehicle457"),
		InlineData("VehicleIRA"),
		InlineData("VehicleRothIRA")]
		public void InvestmentVehicleRemoveCascadeDeletesDependents(string type) {
			var vehicleId = api.Add(type, new OptionsDict { ["investmentVehicleName"] = "test" });
			var tempInvestment = new StockInvestment("");
			context.Portfolio.First().Investments.Add(tempInvestment);
			context.SaveChanges();
			api.AddInvestmentToVehicle(vehicleId, tempInvestment.InvestmentId);

			context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId).InvestmentVehicleModel
				= new VehicleModel();
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
			var vehicleId = api.Add("Vehicle401k", new OptionsDict { ["investmentVehicleName"] = "test" });
			api.UpdateAnalysisOverrides(vehicleId, new OptionsDict { ["test"] = "test" });

			var vehicle = context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			vehicle.AnalysisOptionsOverrides.Should().ContainSingle();
			vehicle.AnalysisOptionsOverrides.ContainsKey("test").Should().BeTrue();
			vehicle.AnalysisOptionsOverrides["test"].Should().Be("test");
		}

		[Fact]
		public void InvestmentVehicleUpdateAnalysisOverridesUpdatesOptionIfExist() {
			var vehicleId = api.Add("Vehicle401k", new OptionsDict { ["investmentVehicleName"] = "test" });
			api.UpdateAnalysisOverrides(vehicleId, new OptionsDict { ["test"] = "test" });
			api.UpdateAnalysisOverrides(vehicleId, new OptionsDict { ["test"] = "test2" });

			var vehicle = context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			vehicle.AnalysisOptionsOverrides.Should().ContainSingle();
			vehicle.AnalysisOptionsOverrides.ContainsKey("test").Should().BeTrue();
			vehicle.AnalysisOptionsOverrides["test"].Should().Be("test2");
		}

		[Fact]
		public void InvestmentVehicleUpdateAnalysisOverridesRemovesOptionIfExistAndNullValue() {
			var vehicleId = api.Add("Vehicle401k", new OptionsDict { ["investmentVehicleName"] = "test" });
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
			var vehicleId = api.Add("Vehicle401k", new OptionsDict { ["investmentVehicleName"] = "test" });
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
			var vehicleId = api.Add("Vehicle401k", new OptionsDict { ["investmentVehicleName"] = "test" });
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
			var vehicleId = api.Add("Vehicle401k", new OptionsDict { ["investmentVehicleName"] = "test" });
			var tempInvestment = new StockInvestment("");
			context.Portfolio.First().Investments.Add(tempInvestment);
			context.SaveChanges();
			api.AddInvestmentToVehicle(vehicleId, tempInvestment.InvestmentId);

			var vehicleId2 = api.Add("Vehicle401k", new OptionsDict { ["investmentVehicleName"] = "test2" });
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
				api.Update(1, new OptionsDict { ["investmentVehicleName"] = "test" });
			};

			act.Should().Throw<ArgumentException>();
		}

		[Fact]
		public void InvestmentVehicleUpdateNameOnlyUpdatesName() {
			var vehicleId = api.Add("Vehicle401k", new OptionsDict { ["investmentVehicleName"] = "test" });
			api.Update(vehicleId, new OptionsDict { ["investmentVehicleName"] = "test2" });

			var vehicle = context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			vehicle.InvestmentVehicleName.Should().Be("test2");
		}


		[Fact]
		public void InvestmentVehicleUpdateCashHoldingsOnlyUpdatesCashHoldings() {
			var vehicleId = api.Add("Vehicle401k", new OptionsDict { ["investmentVehicleName"] = "test" });
			api.Update(vehicleId, new OptionsDict { ["cashHoldings"] = "100" });

			var vehicle = context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			vehicle.CashHoldings.Should().Be(100);
		}

		[Fact]
		public void InvestmentVehicleUpdateMultipleOptionsOnlyUpdatesSpecifiedOptions() {
			var vehicleId = api.Add("Vehicle401k", new OptionsDict { ["investmentVehicleName"] = "test" });
			api.Update(vehicleId, new OptionsDict { ["cashHoldings"] = "100", ["investmentVehicleName"] = "test2" });

			var vehicle = context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			vehicle.CashHoldings.Should().Be(100);
			vehicle.InvestmentVehicleName.Should().Be("test2");
		}

		[Fact]
		public void InvestmentVehicleUpdateInvalidOptionsAreIgnored() {
			var vehicleId = api.Add("Vehicle401k", new OptionsDict { ["investmentVehicleName"] = "test" });
			api.Update(vehicleId, new OptionsDict { ["cashHoldings"] = "100", ["investmentVehicleName"] = "test2", ["test"] = "test" });

			var vehicle = context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			vehicle.CashHoldings.Should().Be(100);
			vehicle.InvestmentVehicleName.Should().Be("test2");
			vehicle.AnalysisOptionsOverrides.Should().BeEmpty();
		}

		public static readonly IEnumerable<object[]> ApiAddData = new List<object[]>{
			new object[]{"Vehicle401k", typeof(Vehicle401k)},
			new object[]{"Vehicle403b", typeof(Vehicle403b)},
			new object[]{"Vehicle457", typeof(Vehicle457)},
			new object[]{"VehicleIRA", typeof(VehicleIRA)},
			new object[]{"VehicleRothIRA", typeof(VehicleRothIRA)},
		};

		[Theory, MemberData(nameof(ApiAddData))]
		public void InvestmentVehicleAddValidTypeCreatesVehicleOfCorrectType(string givenType, Type expected) {
			var vehicleId = api.Add(givenType, new OptionsDict { ["investmentVehicleName"] = "test" });

			var vehicle = context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);

			vehicle.Should().BeOfType(expected);
		}
	}
}
