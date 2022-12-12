using Microsoft.EntityFrameworkCore;

using RetireSimple.Backend.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace RetireSimple.Tests.DomainModel.Sqlite {
    public class InvestmentModelTests : IDisposable {
        InvestmentDBContext context { get; set; }

        private readonly ITestOutputHelper output;

        public InvestmentModelTests(ITestOutputHelper output) {
            context = new InvestmentDBContext(
                new DbContextOptionsBuilder()
                    .UseSqlite("Data Source=InvestmentDB_modeltests.db")
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
        // 1. Investment Model Add
        // 2. Investment Model Remove
        // 3. InvestmentModel FK -> Requires Investment
    }
}
