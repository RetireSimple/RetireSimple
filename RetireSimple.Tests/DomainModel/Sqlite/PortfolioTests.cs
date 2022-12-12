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
    public class PortfolioTests : IDisposable {
        InvestmentDBContext context { get; set; }

        private readonly ITestOutputHelper output;

        public PortfolioTests(ITestOutputHelper output) {
            context = new InvestmentDBContext(
                new DbContextOptionsBuilder()
                    .UseSqlite("Data Source=InvestmentDB.db")
                    //x => x.MigrationsAssembly("RetireSimple.Migrations.Sqlite"))
                    .Options);
            context.Database.Migrate();
            context.Database.EnsureCreated();

            this.output = output;

            //Portfolio Specific Setup
            var profile = new Profile();

            profile.Name = "jack";
            profile.Age = 65;
            profile.Status = true;


            context.Profiles.Add(profile);
            context.SaveChanges();
        }

        public void Dispose() {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        //TODO Tests to add
        //1. Portfolio FK -> Doesn't Cascade any dependents on Delete
        //2. Portfolio FK -> Requires Profile

        [Fact]
        public void TestPortfolioAdd() {
            var portfolio = new Portfolio();
            var portfolio2 = new Portfolio();

            context.Profiles.First(p => p.ProfileId == 1).Portfolios.Add(portfolio);
            context.Profiles.First(p => p.ProfileId == 1).Portfolios.Add(portfolio2);

            context.SaveChanges();
            context.Portfolio.Should().HaveCount(2);
        }

        [Fact]
        public void TestPortfolioRemove() {
            var portfolio = new Portfolio();
            var portfolio2 = new Portfolio();

            context.Profiles.First(p => p.ProfileId == 1).Portfolios.Add(portfolio);
            context.Profiles.First(p => p.ProfileId == 1).Portfolios.Add(portfolio2);
            context.SaveChanges();
            context.Portfolio.Remove(portfolio);
            context.SaveChanges();

            context.Portfolio.Should().HaveCount(1);
        }
    }
}
