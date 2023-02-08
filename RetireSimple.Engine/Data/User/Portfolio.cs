using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RetireSimple.Engine.Data.Investment;
using RetireSimple.Engine.Data.InvestmentVehicle;

namespace RetireSimple.Engine.Data.User {
	public class Portfolio {
		/// <summary>
		/// Primary Key for the Portfolio Table
		/// </summary>
		public int PortfolioId { get; set; }

		public string PortfolioName { get; set; }

		/// <summary>
		/// Foreign Key ID for the <see cref="Profile"/> that contains this Portfolio
		/// </summary>
		public int ProfileId { get; set; }

		/// <summary>
		/// The <see cref="Profile"/> object that contains this Portfolio
		/// </summary>
		public Profile Profile { get; set; }

		//TODO Change to ID Fields?
		public List<InvestmentBase> Investments { get; set; } = new List<InvestmentBase>();
		public List<InvestmentVehicleBase> InvestmentVehicles { get; set; } = new List<InvestmentVehicleBase>();

		/// <summary>
		/// TODO Not Implemented Yet
		/// </summary>
		public void GenerateFullAnalysis() { }
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
				.HasForeignKey(i => i.PortfolioId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(p => p.InvestmentVehicles)
				.WithOne()
				.HasForeignKey(i => i.PortfolioId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(p => p.Profile)
				.WithMany(p => p.Portfolios)
				.HasForeignKey(p => p.ProfileId)
				.OnDelete(DeleteBehavior.Restrict);

			//NOTE this is a placeholder to guarantee an existing Profile/Portfolio until that feature reaches implementation
			builder.HasData(new { PortfolioId = 1, ProfileId = 1, PortfolioName = "Default" });
		}
	}
}
