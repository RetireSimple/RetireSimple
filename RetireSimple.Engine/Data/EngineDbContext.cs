﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Base;
using RetireSimple.Engine.Data.User;

namespace RetireSimple.Engine.Data {
	public class EngineDbContext : DbContext {
		public EngineDbContext(DbContextOptions options) : base(options) { }

		public DbSet<Base.Investment> Investment { get; set; }
		public DbSet<Base.InvestmentVehicle> InvestmentVehicle { get; set; }
		public DbSet<InvestmentModel> InvestmentModel { get; set; }
		public DbSet<Profile> Profile { get; set; }
		public DbSet<Portfolio> Portfolio { get; set; }
		public DbSet<Base.Expense> Expense { get; set; }
		public DbSet<VehicleModel> InvestmentVehicleModel { get; set; }
		public DbSet<PortfolioModel> PortfolioModel { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.ApplyConfiguration(new InvestmentBaseConfiguration());
			modelBuilder.ApplyConfiguration(new InvestmentVehicleBaseConfiguration());
			modelBuilder.ApplyConfiguration(new PortfolioConfig());
			modelBuilder.ApplyConfiguration(new InvestmentModelConfiguration());
			modelBuilder.ApplyConfiguration(new ProfileConfiguration());
			modelBuilder.ApplyConfiguration(new ExpenseBaseConfiguration());
			modelBuilder.ApplyConfiguration(new InvestmentVehicleModelConfiguration());
			modelBuilder.ApplyConfiguration(new PortfolioModelConfiguration());
		}
	}

	public class InvestmentDBContextFactory : IDesignTimeDbContextFactory<EngineDbContext> {
		public EngineDbContext CreateDbContext(string[] args) {
			var optionsBuilder = new DbContextOptionsBuilder<EngineDbContext>();
			optionsBuilder.UseSqlite("Data Source=ef_design.db");

			return new EngineDbContext(optionsBuilder.Options);
		}
	}

}
