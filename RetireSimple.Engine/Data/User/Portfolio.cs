using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RetireSimple.Engine.Data.Analysis;

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

		public List<Base.Investment> Investments { get; set; } = new List<Base.Investment>();
		public List<Base.InvestmentVehicle> InvestmentVehicles { get; set; } = new List<Base.InvestmentVehicle>();

		public PortfolioModel? PortfolioModel { get; set; }
		public DateTime? LastUpdated { get; set; } = DateTime.Now;

		/// <summary>
		/// TODO Not Implemented Yet
		/// </summary>
		public PortfolioModel GenerateFullAnalysis() {
			var investments = Investments;
			foreach (var investment in investments) {
				investment.InvestmentModel ??= investment.PerformAnalysis(new OptionsDict());
			}
			var models = investments.Select(i => i.InvestmentModel).ToList();

			

			//TODO: This is a bit of a hack, but it works for now
			if (!models.Any(m => m is null || m.MaxModelData == null || m.MinModelData == null || m.AvgModelData == null)) {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				var minAggregate = models.Select(m => m.MinModelData).Aggregate((a, b) => a.Zip(b, (x, y) => x + y).ToList());
				var maxAggregate = models.Select(m => m.MaxModelData).Aggregate((a, b) => a.Zip(b, (x, y) => x + y).ToList());
				var avgAggregate = models.Select(m => m.AvgModelData).Aggregate((a, b) => a.Zip(b, (x, y) => x + y).ToList());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
				return new PortfolioModel() {
					PortfolioId = PortfolioId,
					Portfolio = this,
					MaxModelData = maxAggregate.ToList(),
					MinModelData = minAggregate.ToList(),
					AvgModelData = avgAggregate.ToList(),
					LastUpdated = DateTime.Now
				};
			}

			throw new InvalidOperationException("Unable to generate PortfolioModel");
		}
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

			builder.Navigation(p => p.Investments).AutoInclude();
			builder.Navigation(p => p.InvestmentVehicles).AutoInclude();
			builder.Navigation(p => p.PortfolioModel).AutoInclude();

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
