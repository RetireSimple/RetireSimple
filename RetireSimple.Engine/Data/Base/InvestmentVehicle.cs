using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.InvestmentVehicle;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.Data.Base {
	public abstract class InvestmentVehicle {
		public int PortfolioId { get; set; }

		public int InvestmentVehicleId { get; set; }
		public string? InvestmentVehicleName { get; set; }

		public string InvestmentVehicleType { get; set; }

		public List<Investment> Investments { get; set; } = new List<Investment>();

		public int? InvestmentVehicleModelId { get; set; }
		[JsonIgnore] public VehicleModel? InvestmentVehicleModel { get; set; }

		public OptionsDict InvestmentVehicleData { get; set; } = new OptionsDict();

		// A specialized field to hold cash that is not allocated to a specific investment in the vehicle.
		[NotMapped, JsonIgnore]
		public decimal CashHoldings {
			get => decimal.Parse(InvestmentVehicleData["cashHoldings"]);
			set => InvestmentVehicleData["cashHoldings"] = value.ToString();
		}

		public DateTime LastUpdated { get; set; }

		public OptionsDict AnalysisOptionsOverrides { get; set; } = new OptionsDict();

		[JsonIgnore, NotMapped]
		public static readonly OptionsDict DefaultInvestmentVehicleOptions =
			new() {
				["analysisLength"] = "60",
				["vehicleTaxPercentage"] = "0.29", //Tax to apply on contribution/withdrawal/valuation
				["userContributionAmount"] = "0.00",
				["vehicleContributionInvestmentTarget"] = "-1" ///Default, simulates the <see cref="CashHoldings"/> contributions
			};

		/// <summary>
		/// Abstract Declaration for the analysis of a vehicle. While a general
		/// portion of the code remains the same, some of the things such as
		/// applying taxes differ by the vehicle, so this is functionally a template method
		/// </summary>
		/// <param name="options"></param>
		/// <returns></returns>
		public VehicleModel GenerateAnalysis(OptionsDict options) {
			var combinedOptions = MergeOverrideOptions(options);
			var containedModels = GetContainedInvestmentModels(combinedOptions);

			//TODO allow contributing to other investments as well, requires post-processing logic
			var cashSim = SimulateCashContributions(combinedOptions);

			//TODO Pre-process expenses/transfers (from investment in vehicle) to adjust for taxes
			// This will be more relevant when expenses become a factor in analysis.

			var preTaxModel = GeneratePreTaxModels(combinedOptions, containedModels, cashSim);
			var postTaxModel = GeneratePostTaxModels(combinedOptions, containedModels, cashSim);

			var newModel =
				new VehicleModel(InvestmentVehicleId, preTaxModel, postTaxModel);

			//NOTE don't add to EF in this method, that should be an API level responsibility
			return newModel;
		}

		public abstract List<decimal> SimulateCashContributions(OptionsDict options);

		public abstract InvestmentModel GeneratePreTaxModels(OptionsDict options,
			List<InvestmentModel> models,
			List<decimal>? cashContribution = null);

		public abstract InvestmentModel GeneratePostTaxModels(OptionsDict options,
			List<InvestmentModel> models,
			List<decimal>? cashContribution = null);


		//Internal Methods for use in the template method
		private List<InvestmentModel> GetContainedInvestmentModels(OptionsDict options) {
			var models = new List<InvestmentModel>();
			foreach (var investment in Investments) {
				models.Add(investment.InvokeAnalysis(options));
			}
			return models;
		}

		//Internal because used in testing
		internal OptionsDict MergeOverrideOptions(OptionsDict manualOptions) {
			var mergedOptions = new OptionsDict(manualOptions);
			foreach (var key in AnalysisOptionsOverrides.Keys) {
				mergedOptions.TryAdd(key, AnalysisOptionsOverrides[key]);
			}
			foreach (var key in DefaultInvestmentVehicleOptions.Keys) {
				mergedOptions.TryAdd(key, DefaultInvestmentVehicleOptions[key]);
			}
			return mergedOptions;
		}

	}

	public class InvestmentVehicleBaseConfiguration : IEntityTypeConfiguration<InvestmentVehicle> {
		static readonly JsonSerializerOptions options = new() {
			AllowTrailingCommas = true,
			DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
			IncludeFields = true
		};

		public void Configure(EntityTypeBuilder<InvestmentVehicle> builder) {
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
				.HasForeignKey<VehicleModel>(m => m.InvestmentVehicleId)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Property(i => i.LastUpdated)
				.HasColumnType("datetime2(7)");

			builder.Navigation(i => i.Investments)
				.AutoInclude();

			builder.Navigation(i => i.InvestmentVehicleModel)
				.AutoInclude();


#pragma warning disable CS8604 // Possible null reference argument.
			builder.Property(i => i.InvestmentVehicleData)
				.HasConversion(
					v => JsonSerializer.Serialize(v, options),
					v => JsonSerializer.Deserialize<OptionsDict>(v, options) ?? new OptionsDict()
				)
				.Metadata.SetValueComparer(new ValueComparer<OptionsDict>(
					(c1, c2) => c1.SequenceEqual(c2),
					c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
					c => c.ToDictionary(entry => entry.Key, entry => entry.Value)
				));

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