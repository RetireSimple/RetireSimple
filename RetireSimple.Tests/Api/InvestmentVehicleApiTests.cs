using Microsoft.EntityFrameworkCore;

using RetireSimple.Backend.Api;
using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.Services;

using Xunit.Abstractions;

namespace RetireSimple.Tests.Api {
    public class InvestmentVehicleApiTests : IDisposable {

        private readonly ITestOutputHelper output;
        private readonly InvestmentDBContext context;
        private readonly InvestmentVehicleApi api;

        public InvestmentVehicleApiTests(ITestOutputHelper _output) {
            output = _output;
            context = new InvestmentDBContext(
                new DbContextOptionsBuilder()
                    .UseSqlite("Data Source=InvestmentDB_api_vehicle.db")
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
            api.Add(type, "test");

            context.InvestmentVehicle.Should().HaveCount(1);

            var vehicle = context.InvestmentVehicle.First();
            vehicle.InvestmentVehicleName.Should().Be("test");
            vehicle.Investments.Should().HaveCount(1);
        }
    }
}
