using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Expense;
using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.DomainModel.Data.InvestmentVehicleBase;

namespace RetireSimple.Backend.DomainModel.User {
	public class Portfolio {
		public int PortfolioId { get; set; }

		public int ProfileId { get; set; }
		public Profile Profile { get; set; }


		public List<InvestmentBase> Investments { get; set; } = new List<InvestmentBase>();
		public List<InvestmentVehicleBase> InvestmentVehicles { get; set; } = new List<InvestmentVehicleBase>();
		public List<ExpenseBase> Expenses { get; set; } = new List<ExpenseBase>();
		public List<InvestmentTransfer> Transfers { get; set; } = new List<InvestmentTransfer>();

		public void generateFullAnalysis() { }
	}

	public class PortfolioConfig : IEntityTypeConfiguration<Portfolio> {
		public void Configure(EntityTypeBuilder<Portfolio> builder) {
			builder.HasKey(p => p.PortfolioId);

			builder.HasOne(p => p.Profile)
				.WithMany(p => p.Portfolios)
				.HasForeignKey(p => p.ProfileId)
				.IsRequired();

			builder.HasMany(p => p.Investments)
				.WithOne()
				.HasForeignKey(i => i.PortfolioId);
			
			builder.HasMany(p => p.InvestmentVehicles)
				.WithOne()
				.HasForeignKey(i => i.PortfolioId);

			builder.HasMany(p => p.Expenses)
				.WithOne()
				.HasForeignKey(e => e.PorfolioId)
				.IsRequired();
			
			builder.HasMany(p => p.Transfers)
				.WithOne()
				.HasForeignKey(t => t.PorfolioId);

			builder.HasOne(p => p.Profile)
				.WithMany(p => p.Portfolios)
				.HasForeignKey(p => p.ProfileId)
				.OnDelete(DeleteBehavior.Restrict);

		}
	}
}
