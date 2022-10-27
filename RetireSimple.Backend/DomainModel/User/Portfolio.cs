using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Expense;
using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.DomainModel.Data.InvestmentVehicle;

namespace RetireSimple.Backend.DomainModel.User {
	public class Portfolio {
		public int PortfolioId { get; set; }

		public int ProfileId { get; set; }
		public Profile Profile { get; set; }


		public List<InvestmentBase> Investments { get; set; } = new List<InvestmentBase>();
		List<InvestmentVehicleBase> InvestmentVehicles { get; set; } = new List<InvestmentVehicleBase>();
		List<ExpenseBase> Expenses { get; set; } = new List<ExpenseBase>();
		List<InvestmentTransfer> Transfers { get; set; } = new List<InvestmentTransfer>();

		public void generateFullAnalysis() { }
	}

	public class PortfolioConfig : IEntityTypeConfiguration<Portfolio> {
		public void Configure(EntityTypeBuilder<Portfolio> builder) {
			//TODO setup relationship schema
			builder.HasKey(p => p.PortfolioId);
			builder.HasOne(p => p.Profile).WithMany(p => p.Portfolios).HasForeignKey(p => p.ProfileId);
			//builder.HasMany(p => p.Investments).WithOne(i => i.Portfolio);

		}
	}
}
