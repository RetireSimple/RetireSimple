using Microsoft.EntityFrameworkCore;
using RetireSimple.Backend.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace RetireSimple.Tests.DomainModel.Sqlite {
    public class InvestmentTransfersTests {
        InvestmentDBContext context { get; set; }

        private readonly ITestOutputHelper output;

        public InvestmentTransfersTests(ITestOutputHelper output) {
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
        // 1. InvestmentTransfer Model Add
        // 2. InvestmentTransfer Model Remove
        // 3. InvestmentTransfer Model FK -> Requires Investments + Portfolio

    }
}
