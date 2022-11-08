//These tests use an existing Sqlite DB to test if value convereters are implemented as expected
//for data fields, as those rely on a specific method

using Microsoft.EntityFrameworkCore;

using RetireSimple.Backend.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace RetireSimple.Tests.DomainModel.Sqlite {
    public class SqliteValueConversionTest {

        InvestmentDBContext context { get; set; }
        private readonly ITestOutputHelper output;

        public SqliteValueConversionTest(ITestOutputHelper output) {
            //TODO create a new Sqlite DB for this test with seeded test data
            context = new InvestmentDBContext(
                new DbContextOptionsBuilder()
                    .UseSqlite("Data Source=InvestmentDB.db",
                    x => x.MigrationsAssembly("RetireSimple.Migrations.Sqlite"))
                    .Options);

            context.Database.Migrate();
            context.Database.EnsureCreated();

            this.output = output;
        }



    }
}
