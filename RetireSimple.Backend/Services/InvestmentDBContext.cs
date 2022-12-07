using Microsoft.EntityFrameworkCore;

using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Expense;
using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.DomainModel.User;


namespace RetireSimple.Backend.Services {
	public class InvestmentDBContext : DbContext {
		public InvestmentDBContext(DbContextOptions options) : base(options) {
		}

		public DbSet<InvestmentBase> Investments { get; set; }
		//DbSet<InvestmentVehicleBase> InvestmentVehicles { get; set; }
		public DbSet<InvestmentModel> InvestmentModels { get; set; }
		public DbSet<Profile> Profiles { get; set; }
		public DbSet<Portfolio> Portfolio { get; set; }
		public DbSet<ExpenseBase> Expenses { get; set; }
		public DbSet<InvestmentTransfer> InvestmentTransfers { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.ApplyConfiguration(new InvestmentBaseConfiguration());
		
			modelBuilder.ApplyConfiguration(new PortfolioConfig());

			modelBuilder.ApplyConfiguration(new InvestmentModelConfiguration());

			modelBuilder.ApplyConfiguration(new ProfileConfiguration());
			modelBuilder.ApplyConfiguration(new ExpenseBaseConfiguration());
			modelBuilder.ApplyConfiguration(new InvestmentTransferConfiguration());


		}

	}

}
