namespace RetireSimple.Tests.DomainModel {
	public class ProfileTests : IDisposable {

		EngineDbContext Context { get; set; }

		public ProfileTests() {
			Context = new EngineDbContext(
				new DbContextOptionsBuilder()
					.UseSqlite("Data Source=testing_profiles.db")
					.Options);
			Context.Database.Migrate();
			Context.Database.EnsureCreated();
		}

		public void Dispose() {
			Context.Database.EnsureDeleted();
			Context.Dispose();
		}

		[Fact]
		public void TestProfileAdd() {
			var profile = new Profile {
				Name = "jack",
				Age = 65,
				Status = true
			};
			var profile2 = new Profile {
				Name = "jake",
				Age = 25,
				Status = false
			};

			Context.Profile.Add(profile);
			Context.Profile.Add(profile2);

			Context.SaveChanges();
			Context.Profile.Should().HaveCount(3);
		}

		[Fact]
		public void TestPortfolioFKConstraint() {
			var portfolio = new Portfolio { PortfolioName = "test" };

			var profile = new Profile {
				Name = "jack",
				Age = 65,
				Status = true
			};

			Context.Profile.Add(profile);
			Context.SaveChanges();
			Context.Profile.First(p => p.ProfileId == 1).Portfolios.Add(portfolio);
			Context.SaveChanges();

			Action act = () => {
				Context.Profile.Remove(profile);
			};

			act.Should().NotThrow();
		}

		[Fact]
		public void TestProfileRemove() {
			var profile = new Profile {
				Name = "jack",
				Age = 65,
				Status = true
			};
			var profile2 = new Profile {
				Name = "jake",
				Age = 25,
				Status = false
			};

			Context.Profile.Add(profile);
			Context.Profile.Add(profile2);
			Context.SaveChanges();

			Context.Profile.Remove(profile);
			Context.SaveChanges();

			Context.Profile.Should().HaveCount(2);
		}

	}
}
