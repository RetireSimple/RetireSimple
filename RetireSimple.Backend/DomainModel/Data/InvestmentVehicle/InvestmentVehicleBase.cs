using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.DomainModel.User;
using System.Text.Json;

namespace RetireSimple.Backend.DomainModel.Data.InvestmentVehicle {
	public abstract class InvestmentVehicleBase {
		public int PortfolioId { get; set; }

		public int InvestmentVehicleId { get; set; }

		public string InvestmentVehicleType { get; set; }

		public List<InvestmentBase> Investments { get; set; }

		public OptionsDict AnalysisOptionsOverrides { get; set; } = new OptionsDict();

		public InvestmentVehicleBase() {
			Investments = new List<InvestmentBase>();
		}

		//The modularity of investment module analysis is restricted to the type of vehicle for logic
		//constraints that are not yet clear.  This is a placeholder for future work.
		public abstract InvestmentModel GenerateAnalysis(OptionsDict options);
	}

	public class InvestmentVehicleBaseConfiguration : IEntityTypeConfiguration<InvestmentVehicleBase> {
		static JsonSerializerOptions options = new JsonSerializerOptions {
			AllowTrailingCommas = true,
			DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
			IncludeFields = true
		};

		public void Configure(EntityTypeBuilder<InvestmentVehicleBase> builder) {
			builder.HasKey(i => i.InvestmentVehicleId);

			builder.HasDiscriminator(i => i.InvestmentVehicleType)
				.HasValue<Vehicle401k>("401k")
				.HasValue<Vehicle403b>("403b")
				.HasValue<Vehicle457>("457")
				.HasValue<VehicleIRA>("IRA")
				.HasValue<VehicleRothIRA>("RothIRA");

			builder.HasMany(i => i.Investments)
				.WithOne()
				.IsRequired(false)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Property(i => i.AnalysisOptionsOverrides)
				.HasConversion(
					v => JsonSerializer.Serialize(v, options),
					v => JsonSerializer.Deserialize<OptionsDict>(v, options) ?? new OptionsDict()
				)
				.Metadata.SetValueComparer(new ValueComparer<OptionsDict>(
					(c1, c2) => c1.SequenceEqual(c2),
					c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
					c => c.ToDictionary(entry => entry.Key, entry => entry.Value)
				));
		}
	}

}