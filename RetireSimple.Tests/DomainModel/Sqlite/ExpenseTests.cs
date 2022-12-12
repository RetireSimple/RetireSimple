using Microsoft.EntityFrameworkCore;

using RetireSimple.Backend.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace RetireSimple.Tests.DomainModel.Sqlite {
    public class ExpenseTests : IDisposable {

        InvestmentDBContext context { get; set; }

        private readonly ITestOutputHelper output;

        public ExpenseTests(ITestOutputHelper output) {
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

        //Tests to Add:
        // 1. Expense Model Add
        // 2. Expense Model Remove
        // 3. ExpenseModel FK -> Requires Investment + Portfolio
    }
}
