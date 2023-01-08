using Microsoft.EntityFrameworkCore;

using RetireSimple.Backend.DomainModel.Data.Expense;
using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.DomainModel.User;
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
                    .UseSqlite("Data Source=InvestmentDB_expensetests.db")
                    .Options);
            context.Database.Migrate();
            context.Database.EnsureCreated();

            this.output = output;

            //Expense Specific Setup

            var profile = new Profile();
            profile.Name = "jack";
            profile.Age = 65;
            profile.Status = true;

            var portfolio = new Portfolio();

            context.Profiles.Add(profile);
            context.SaveChanges();
            context.Profiles.First(p => p.ProfileId == 1).Portfolios.Add(portfolio);
            context.SaveChanges();

            var investment = new StockInvestment("test");
            investment.StockPrice = 100;
            investment.StockQuantity = 10;
            investment.StockTicker = "TST";
            context.Portfolio.First(p => p.PortfolioId == 1).Investments.Add(investment);
            context.SaveChanges();
        }

        public void Dispose() {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        //Tests to Add:
        // 1. Expense Model Add

        [Fact]
        public void TestExpenseAdd()
        {
            //TODO: add manual constraint of Expense's investments
            var expense = new OneTimeExpense();
            expense.Amount = 100.0;

            context.Portfolio.First(p => p.PortfolioId == 1).Expenses.Add(expense);
            expense.SourceInvestment = context.Investments.First(i => i.InvestmentId == 1);
            context.SaveChanges();
            Assert.Equal(1, context.Expenses.Count());

        }
        // 2. Expense Model Remove

        [Fact]
        public void TestExpenseRemove()
        {
            var expense = new OneTimeExpense();
            expense.Amount = 100.0;

            context.Portfolio.First(p => p.PortfolioId == 1).Expenses.Add(expense);
            expense.SourceInvestment = context.Investments.First(i => i.InvestmentId == 1);
            context.SaveChanges();
            context.Portfolio.First(p => p.PortfolioId == 1).Expenses.Remove(expense);
            context.SaveChanges();
            Assert.Equal(0, context.Expenses.Count());
        }
        // 3. ExpenseModel FK -> Requires Investment + Portfolio

        [Fact]
        public void TestExpenseFKConstraint()
        {
            var expense = new OneTimeExpense();
            expense.Amount = 100.0;

            //context.Portfolio.First(p => p.PortfolioId == 1).Expenses.Add(expense);
            //expense.SourceInvestment = context.Investments.First(i => i.InvestmentId == 1);

            Action act = () =>
            {
                context.Expenses.Add(expense);
                context.SaveChanges();
            };

            act.Should().Throw<DbUpdateException>();
        }
    }
}
