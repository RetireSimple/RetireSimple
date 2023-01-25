namespace RetireSimple.Tests.DomainModel {
	public class ProfileTests : IDisposable {

		EngineDbContext context { get; set; }

		private readonly ITestOutputHelper output;

		public ProfileTests(ITestOutputHelper output) {
			context = new EngineDbContext(
				new DbContextOptionsBuilder()
					.UseSqlite("Data Source=testing_profiles.db")
					.Options);
			context.Database.Migrate();
			context.Database.EnsureCreated();

			this.output = output;
		}

		public void Dispose() {
			context.Database.EnsureDeleted();
			context.Dispose();
		}

		[Fact]
		public void TestProfileAdd() {
			var profile = new Profile();
			var profile2 = new Profile();
			profile.Name = "jack";
			profile.Age = 65;
			profile.Status = true;

			profile2.Name = "jake";
			profile2.Age = 25;
			profile2.Status = false;

			context.Profile.Add(profile);
			context.Profile.Add(profile2);

			context.SaveChanges();
			context.Profile.Should().HaveCount(3);
		}

		[Fact]
		public void TestPortfolioFKConstraint() {
			var portfolio = new Portfolio();
			portfolio.PortfolioName = "test";

			var profile = new Profile();
			profile.Name = "jack";
			profile.Age = 65;
			profile.Status = true;

			context.Profile.Add(profile);
			context.SaveChanges();
			context.Profile.First(p => p.ProfileId == 1).Portfolios.Add(portfolio);
			context.SaveChanges();

			Action act = () => {
				context.Profile.Remove(profile);
			};

			act.Should().NotThrow();
		}

		[Fact]
		public void TestProfileRemove() {
			var profile = new Profile();
			var profile2 = new Profile();

			profile.Name = "jack";
			profile.Age = 65;
			profile.Status = true;

			profile2.Name = "jake";
			profile2.Age = 25;
			profile2.Status = false;

			context.Profile.Add(profile);
			context.Profile.Add(profile2);
			context.SaveChanges();

			context.Profile.Remove(profile);
			context.SaveChanges();

			context.Profile.Should().HaveCount(2);
		}

	}
}
