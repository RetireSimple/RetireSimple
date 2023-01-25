using Microsoft.EntityFrameworkCore;

using RetireSimple.Engine.Data.Expense;
using RetireSimple.Engine.Data.Investment;
using RetireSimple.Engine.Data.InvestmentVehicle;
using RetireSimple.Engine.Data.User;

namespace RetireSimple.Engine.Data {
	public class InvestmentDBContext : DbContext {
		public InvestmentDBContext(DbContextOptions options) : base(options) {
		}

		public DbSet<InvestmentBase> Investment { get; set; }
		public DbSet<InvestmentVehicleBase> InvestmentVehicle { get; set; }
		public DbSet<InvestmentModel> InvestmentModel { get; set; }
		public DbSet<Profile> Profile { get; set; }
		public DbSet<Portfolio> Portfolio { get; set; }
		public DbSet<ExpenseBase> Expense { get; set; }
		public DbSet<InvestmentTransfer> InvestmentTransfer { get; set; }
		public DbSet<InvestmentVehicleModel> InvestmentVehicleModel { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.ApplyConfiguration(new InvestmentBaseConfiguration());
			modelBuilder.ApplyConfiguration(new InvestmentVehicleBaseConfiguration());
			modelBuilder.ApplyConfiguration(new PortfolioConfig());
			modelBuilder.ApplyConfiguration(new InvestmentModelConfiguration());
			modelBuilder.ApplyConfiguration(new ProfileConfiguration());
			modelBuilder.ApplyConfiguration(new ExpenseBaseConfiguration());
			modelBuilder.ApplyConfiguration(new InvestmentTransferConfiguration());
			modelBuilder.ApplyConfiguration(new InvestmentVehicleModelConfiguration());


		}

	}

}
