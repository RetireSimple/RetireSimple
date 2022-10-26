using Microsoft.EntityFrameworkCore;

using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.DomainModel.User;


namespace RetireSimple.Backend.Services {
	public class InvestmentDBContext : DbContext {
		public DbSet<InvestmentBase> Investments { get; set; }
		//DbSet<InvestmentVehicleBase> InvestmentVehicles { get; set; }
		public DbSet<InvestmentModel> InvestmentModels { get; set; }
		//DbSet<Profile> Profiles { get; set; }
		//DbSet<Portfolio> Portfolios { get; set; }
		//DbSet<ExpenseBase> Expenses { get; set; }
		//DbSet<InvestmentTransfer> InvestmentTransfers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.ApplyConfiguration(new InvestmentBaseConfiguration());
			//modelBuilder.ApplyConfiguration(new InvestmentVehicleBaseConfiguration());
			//modelBuilder.ApplyConfiguration(new PortfolioConfig());

			modelBuilder.ApplyConfiguration(new InvestmentModelConfiguration());
			//modelBuilder.Entity<Profile>().ToTable("Profiles");
			//modelBuilder.Entity<ExpenseBase>().ToTable("Expenses");
			//modelBuilder.Entity<InvestmentTransfer>().ToTable("InvestmentTransfers");


		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder.UseSqlite("Data Source=InvestmentDB.db");
		}

	}
}
