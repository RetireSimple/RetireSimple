using Microsoft.EntityFrameworkCore;

using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.User;

using System.Text.Json;

namespace RetireSimple.Backend.Services {
    public class InvestmentDBContext : DbContext {
        DbSet<InvestmentBase> Investments { get; set; }
        DbSet<InvestmentVehicleBase> InvestmentVehicles { get; set; }
        DbSet<InvestmentModel> InvestmentModels { get; set; }
        DbSet<Profile> Profiles { get; set; }
        DbSet<Portfolio> Portfolios { get; set; }
        DbSet<ExpenseBase> Expenses { get; set; }
        DbSet<InvestmentTransfer> InvestmentTransfers { get; set; }

        public InvestmentDBContext(DbContextOptions<InvestmentDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.ApplyConfiguration<InvestmentBase>(new InvestmentBaseConfiguration());


			modelBuilder.Entity<InvestmentVehicleBase>().ToTable("InvestmentVehicles");
            modelBuilder.Entity<InvestmentModel>().ToTable("InvestmentModels");
            modelBuilder.Entity<Profile>().ToTable("Profiles");
            modelBuilder.Entity<Portfolio>().ToTable("Portfolios");
            modelBuilder.Entity<ExpenseBase>().ToTable("Expenses");
            modelBuilder.Entity<InvestmentTransfer>().ToTable("InvestmentTransfers");


        }


    }
}
