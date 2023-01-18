using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RetireSimple.Backend.DomainModel.Data.Investment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RetireSimple.Backend.DomainModel.Data.InvestmentVehicle {
	public abstract class InvestmentVehicleBase {

		public int PortfolioId { get; set; }

		public int InvestmentVehicleId { get; set; }
		public string? InvestmentVehicleName { get; set; }

		public string InvestmentVehicleType { get; set; }

		public List<InvestmentBase> Investments { get; set; } = new List<InvestmentBase>();

		public int? InvestmentVehicleModelId { get; set; }
		[JsonIgnore]
		public InvestmentVehicleModel? InvestmentVehicleModel { get; set; }

		public int? CashInvestmentId { get; set; }
		[JsonIgnore]
		public InvestmentBase? CashInvestment { get; set; }

		public OptionsDict AnalysisOptionsOverrides { get; set; } = new OptionsDict();

		[JsonIgnore, NotMapped]
		public static readonly OptionsDict DefaultInvestmentVehicleOptions = new OptionsDict() {
			["VehicleTaxPercentage"] = "0.29"   //Tax to apply on contribution/withdrawal/valuation
		};


		/// <summary>
		/// Abstract Declaration for the analysis of a vehicle. While a general
		/// portion of the code remains the same, some of the things such as 
		/// applying taxes differ by the vehicle, so we provide a method to 
		/// override based on the vehicle.
		/// </summary>
		/// <param name="options"></param>
		/// <returns></returns>
		public abstract InvestmentModel GenerateAnalysis(OptionsDict options);

		/// <summary>
		/// This method should be used instead of direct property access,
		/// it automatically excludes the vehicle's default 
		/// <see cref="CashInvestment"/> used to account for contributions
		/// </summary>
		/// <returns></returns>
		public List<InvestmentBase> GetInvestments() {
			return Investments.Where(i => i.InvestmentId != CashInvestmentId).ToList();
		}
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
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(i => i.InvestmentVehicleModel)
				.WithOne()
				.HasForeignKey<InvestmentVehicleModel>(m => m.InvestmentVehicleId)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(i => i.CashInvestment)
				.WithOne()
				.IsRequired(false)
				.OnDelete(DeleteBehavior.Cascade);

#pragma warning disable CS8604 // Possible null reference argument.
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
#pragma warning restore CS8604 // Possible null reference argument.

		}
	}

}