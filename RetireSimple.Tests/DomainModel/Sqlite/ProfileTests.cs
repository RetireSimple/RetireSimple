using Microsoft.EntityFrameworkCore;

using RetireSimple.Backend.DomainModel.User;
using RetireSimple.Backend.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace RetireSimple.Tests.DomainModel.Sqlite {
    public class ProfileTests : IDisposable {

        InvestmentDBContext context { get; set; }

        private readonly ITestOutputHelper output;

        public ProfileTests(ITestOutputHelper output) {
            context = new InvestmentDBContext(
                new DbContextOptionsBuilder()
                    .UseSqlite("Data Source=InvestmentDB.db")
                    //x => x.MigrationsAssembly("RetireSimple.Migrations.Sqlite"))
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

            context.Profiles.Add(profile);
            context.Profiles.Add(profile2);

            context.SaveChanges();
            context.Profiles.Should().HaveCount(2);
        }

        [Fact]
        public void TestPortfolioFKConstraint() {
            var portfolio = new Portfolio();
            var profile = new Profile();

            profile.Name = "jack";
            profile.Age = 65;
            profile.Status = true;

            context.Profiles.Add(profile);
            context.SaveChanges();
            context.Profiles.First(p => p.ProfileId == 1).Portfolios.Add(portfolio);
            context.SaveChanges();


            Action act = () => {
                context.Profiles.Remove(profile);
            };

            act.Should().Throw<InvalidOperationException>();
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



            context.Profiles.Add(profile);
            context.Profiles.Add(profile2);

            context.SaveChanges();

            context.Profiles.Remove(profile);
            context.SaveChanges();

            context.Profiles.Should().HaveCount(1);
        }

    }
}
