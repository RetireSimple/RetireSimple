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
				api.UpdateAnalysisOverrides(1, "test", "test");
			};

			act.Should().Throw<ArgumentException>();
		}

		[Fact]
		public void InvestmentVehicleUpdateAnalysisOverridesThrowsIfNullValueAndNoExistingKey() {
			api.Add("401k", "test");

			Action act = () => {
				api.UpdateAnalysisOverrides(1, "test", null);
			};

			act.Should().Throw<ArgumentException>();
		}

		[Fact]
		public void InvestmentVehicleUpdateAnalysisOverridesAddsOptionIfNotExist() {
			var vehicleId = api.Add("401k", "test");
			api.UpdateAnalysisOverrides(vehicleId, "test", "test");

			var vehicle = context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			vehicle.AnalysisOptionsOverrides.Should().ContainSingle();
			vehicle.AnalysisOptionsOverrides.ContainsKey("test").Should().BeTrue();
			vehicle.AnalysisOptionsOverrides["test"].Should().Be("test");
		}

		[Fact]
		public void InvestmentVehicleUpdateAnalysisOverridesUpdatesOptionIfExist() {
			var vehicleId = api.Add("401k", "test");
			api.UpdateAnalysisOverrides(vehicleId, "test", "test");
			api.UpdateAnalysisOverrides(vehicleId, "test", "test2");

			var vehicle = context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			vehicle.AnalysisOptionsOverrides.Should().ContainSingle();
			vehicle.AnalysisOptionsOverrides.ContainsKey("test").Should().BeTrue();
			vehicle.AnalysisOptionsOverrides["test"].Should().Be("test2");
		}

		[Fact]
		public void InvestmentVehicleUpdateAnalysisOverridesRemovesOptionIfExistAndNullValue() {
			var vehicleId = api.Add("401k", "test");
			api.UpdateAnalysisOverrides(vehicleId, "test", "test");
			api.UpdateAnalysisOverrides(vehicleId, "test", null);

			var vehicle = context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			vehicle.AnalysisOptionsOverrides.Should().BeEmpty();
			vehicle.AnalysisOptionsOverrides.ContainsKey("test").Should().BeFalse();

		}
	}
}
