using RetireSimple.Engine.Data.Analysis;

namespace RetireSimple.Tests.DomainModel {
	public class VehicleModelTests : IDisposable {
		EngineDbContext Context { get; set; }

		public VehicleModelTests() {
			Context = new EngineDbContext(
				new DbContextOptionsBuilder()
					.UseSqlite("Data Source=testing_vmodel.db")
					.Options);
			Context.Database.Migrate();
			//context.Database.EnsureCreated();

			var vehicle = new Vehicle401k();
			Context.Portfolio.First(p => p.PortfolioId == 1).InvestmentVehicles.Add(vehicle);
			Context.SaveChanges();
		}

		public void Dispose() {
			Context.Database.EnsureDeleted();
			Context.Dispose();
		}

		[Fact]
		public void TestInvestmentVehicleModelAdd() {
			VehicleModel model = new();
			Context.Portfolio.First(p => p.PortfolioId == 1)
				.InvestmentVehicles.First(i => i.InvestmentVehicleId == 1)
				.InvestmentVehicleModel = model;

			Context.SaveChanges();

			Assert.Single(Context.InvestmentVehicleModel);
		}

		[Fact]
		public void TestInvestmentModelRemove() {
			VehicleModel model = new();
			Context.Portfolio.First(p => p.PortfolioId == 1)
				.InvestmentVehicles.First(i => i.InvestmentVehicleId == 1)
				.InvestmentVehicleModel = model;
			Context.SaveChanges();


			Context.InvestmentVehicleModel.Remove(Context.InvestmentVehicleModel.First());
			Context.SaveChanges();

			Assert.Empty(Context.InvestmentVehicleModel);
		}

		[Fact]
		public void TestInvestmentModelFKConstraint() {
			VehicleModel model = new();

			Action act = () => {
				Context.InvestmentVehicleModel.Add(model);
				Context.SaveChanges();
			};

			act.Should().Throw<DbUpdateException>();
		}
	}
}
