using Microsoft.EntityFrameworkCore;

using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Expense;
using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.DomainModel.User;
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes

namespace RetireSimple.Backend.Services {
	public class InvestmentDBContext : DbContext {
		public InvestmentDBContext(DbContextOptions options) : base(options) {
		}

		public DbSet<InvestmentBase> Investments { get; set; }
		//DbSet<InvestmentVehicleBase> InvestmentVehicles { get; set; }
		public DbSet<InvestmentModel> InvestmentModels { get; set; }
		public DbSet<Profile> Profiles { get; set; }
<<<<<<< Updated upstream
		//DbSet<Portfolio> Portfolios { get; set; }
		DbSet<ExpenseBase> Expenses { get; set; }
		DbSet<InvestmentTransfer> InvestmentTransfers { get; set; }
=======
		public DbSet<Portfolio> Portfolios { get; set; }
		public DbSet<ExpenseBase> Expenses { get; set; }
		public DbSet<InvestmentTransfer> InvestmentTransfers { get; set; }
>>>>>>> Stashed changes

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.ApplyConfiguration(new InvestmentBaseConfiguration());
			//modelBuilder.ApplyConfiguration(new InvestmentVehicleBaseConfiguration());
			modelBuilder.ApplyConfiguration(new PortfolioConfig());

<<<<<<< Updated upstream
			modelBuilder.ApplyConfiguration(new InvestmentModelConfiguration());
=======
			modelBuilder.ApplyConfiguration(new InvestmentModelConfiguration());
			//modelBuilder.Entity<Profile>().ToTable("Profiles");
>>>>>>> Stashed changes
			modelBuilder.ApplyConfiguration(new ProfileConfiguration());
			modelBuilder.ApplyConfiguration(new ExpenseBaseConfiguration());
			modelBuilder.ApplyConfiguration(new InvestmentTransferConfiguration());


		}

	}

}
