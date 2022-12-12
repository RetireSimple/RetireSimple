using Microsoft.EntityFrameworkCore;
using RetireSimple.Backend.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace RetireSimple.Tests.DomainModel.Sqlite {
    public class InvestmentVehicleTests : IDisposable {
        InvestmentDBContext context { get; set; }

        private readonly ITestOutputHelper output;

        public InvestmentVehicleTests(ITestOutputHelper output) {
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

        //TODO Tests to add
        //1. InvestmentVehicle Model Add
        //2. InvestmentVehicle Model Remove
        //3. InvestmentVehicle Model FK -> Requires Portfolio
        //4. InvestmentVehicle Model FK -> Doesn't Cascade Investments
        //5. Discriminator Configuration (Adding different types of investment Vehicles)

    }
}
