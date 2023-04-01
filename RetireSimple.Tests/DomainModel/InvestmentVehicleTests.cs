using RetireSimple.Engine.Data.Base;

namespace RetireSimple.Tests.DomainModel {
	public class InvestmentVehicleTests : IDisposable {
		EngineDbContext Context { get; set; }

		public InvestmentVehicleTests() {
			Context = new EngineDbContext(
				new DbContextOptionsBuilder()
					.UseSqlite("Data Source=testing_vehicle.db")
					.Options);
			Context.Database.Migrate();
			Context.Database.EnsureCreated();

			var investment = new StockInvestment("") {
				StockPrice = 100,
				StockQuantity = 10,
				StockTicker = "TST"
			};
			Context.Portfolio.First(p => p.PortfolioId == 1).Investments.Add(investment);
			Context.SaveChanges();
		}

		public void Dispose() {
			Context.Database.EnsureDeleted();
			Context.Dispose();
		}

		[Fact]
		public void TestInvestmentVehicleAdd() {
			InvestmentVehicle vehicle = new Vehicle403b();
			Context.Portfolio.First(p => p.PortfolioId == 1).InvestmentVehicles.Add(vehicle);
			Context.SaveChanges();
			vehicle.Investments.Add(Context.Investment.First(i => i.InvestmentId == 1));
			Context.SaveChanges();

			Assert.Single(Context.InvestmentVehicle);
		}

		[Fact]
		public void TestInvestmentVehicleRemove() {
			InvestmentVehicle vehicle = new Vehicle403b();
			Context.Portfolio.First(p => p.PortfolioId == 1).InvestmentVehicles.Add(vehicle);
			Context.SaveChanges();
			vehicle.Investments.Add(Context.Investment.First(i => i.InvestmentId == 1));
			Context.SaveChanges();

			Context.InvestmentVehicle.Remove(vehicle);
			Context.SaveChanges();

			Assert.Equal(0, Context.InvestmentVehicle.Count());
		}

		[Fact]
		public void TestInvestmentVehicleFKConstraintPortfolio() {
			InvestmentVehicle vehicle = new Vehicle403b();

			Action act = () => {
				Context.InvestmentVehicle.Add(vehicle);
				Context.SaveChanges();
			};

			act.Should().Throw<DbUpdateException>();
		}

		[Fact]
		public void TestInvestmentVehicleFKConstraintInvestmentDeleteCascades() {
			InvestmentVehicle vehicle = new Vehicle403b();
			Context.Portfolio.First(p => p.PortfolioId == 1).InvestmentVehicles.Add(vehicle);
			vehicle.Investments.Add(Context.Investment.First(i => i.InvestmentId == 1));
			Context.SaveChanges();

			Action act = () => {
				Context.InvestmentVehicle.Remove(vehicle);
				Context.SaveChanges();
			};

			act.Should().NotThrow();
			Context.Investment.Should().BeEmpty();
			Context.InvestmentVehicle.Should().BeEmpty();
		}

		//TODO add cascade test for Vehicle models

	}
}
