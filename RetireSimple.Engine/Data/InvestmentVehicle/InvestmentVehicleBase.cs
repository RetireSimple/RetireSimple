using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RetireSimple.Engine.Data.Investment;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.Data.InvestmentVehicle {
	public abstract class InvestmentVehicleBase {
		public int PortfolioId { get; set; }

		public int InvestmentVehicleId { get; set; }
		public string? InvestmentVehicleName { get; set; }

		public string InvestmentVehicleType { get; set; }

		public List<InvestmentBase> Investments { get; set; } = new List<InvestmentBase>();

		public int? InvestmentVehicleModelId { get; set; }
		[JsonIgnore] public InvestmentVehicleModel? InvestmentVehicleModel { get; set; }

		public OptionsDict InvestmentVehicleData { get; set; } = new OptionsDict();

		// A specialized field to hold cash that is not allocated to a specific investment in the vehicle.
		[NotMapped, JsonIgnore]
		public decimal CashHoldings {
			get => decimal.Parse(this.InvestmentVehicleData["cashHoldings"]);
			set => this.InvestmentVehicleData["cashHoldings"] = value.ToString();
		}

		public DateTime LastUpdated { get; set; }

		public OptionsDict AnalysisOptionsOverrides { get; set; } = new OptionsDict();

		[JsonIgnore, NotMapped]
		public static readonly OptionsDict DefaultInvestmentVehicleOptions =
			new() {
				["VehicleTaxPercentage"] = "0.29", //Tax to apply on contribution/withdrawal/valuation
				["VehicleMonthlyContribution"] = "0.00m",
				["VehicleContributionInvestmentTarget"] = "-1" ///Default, simulates the <see cref="CashHoldings"/> contributions
			};

		/// <summary>
		/// Abstract Declaration for the analysis of a vehicle. While a general
		/// portion of the code remains the same, some of the things such as
		/// applying taxes differ by the vehicle, so this is functionally a template method
		/// </summary>
		/// <param name="options"></param>
		/// <returns></returns>
		public InvestmentVehicleModel GenerateAnalysis(OptionsDict options) {
			var combinedOptions = MergeOverrideOptions(options);
			var containedModels = GetContainedInvestmentModels(combinedOptions);

			//TODO allow contributing to other investments as well, requires post-processing logic
			var cashSim = SimulateCashContributions(combinedOptions);

			//TODO Pre-process expenses/transfers (from investment in vehicle) to adjust for taxes
			// This will be more relevant when expenses become a factor in analysis.

			var preTaxModel = GeneratePreTaxModels(options, containedModels, cashSim);
			var postTaxModel = GeneratePostTaxModels(options, containedModels, cashSim);

			var newModel =
				new InvestmentVehicleModel(InvestmentVehicleId, preTaxModel, postTaxModel);

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

		private List<InvestmentModel> GetContainedInvestmentModels(OptionsDict options) {
			var models = new List<InvestmentModel>();
			foreach (var investment in Investments) {
				models.Add(investment.InvokeAnalysis(options));
			}

			return models;
		}

		private OptionsDict MergeOverrideOptions(OptionsDict manualOptions) {
			var mergedOptions = new OptionsDict(manualOptions);

			foreach (var key in AnalysisOptionsOverrides.Keys) {
				mergedOptions.TryAdd(key, AnalysisOptionsOverrides[key]);
			}

			foreach (var key in DefaultInvestmentVehicleOptions.Keys) {
				mergedOptions.TryAdd(key, DefaultInvestmentVehicleOptions[key]);
			}

			return mergedOptions;
		}


		///The following methods provide common logic for implementing the template method. You can
		///have specific modules wrap around these if the logic applies (even partially).
		public static InvestmentModel GeneratePreTaxModelDefaultAfterTaxVehicle(OptionsDict options,
			List<InvestmentModel> models,
			List<decimal>? cashContribution = null) {
			//The logic here is a bit confusing at first, but here is an explanation of the transforms
			var minModel = Enumerable.Range(0, models[0].MinModelData.Count) //Project the total length of the model with indexes
									.Select(model =>                                    //For each index,
										models.Select(m => m.MinModelData[model]).Sum()); //project each model's value at that index and sum
			var maxModel = Enumerable.Range(0, models[0].MaxModelData.Count)
									.Select(model =>
										models.Select(m => m.MaxModelData[model]).Sum());
			var avgModel = Enumerable.Range(0, models[0].AvgModelData.Count)
									.Select(model =>
										models.Select(m => m.AvgModelData[model]).Sum());

			//If cash-based contributions exist, transform all models to include them
			if (cashContribution != null) {
				minModel = minModel.Select((val, idx) => val + cashContribution[idx]);
				maxModel = maxModel.Select((val, idx) => val + cashContribution[idx]);
				avgModel = avgModel.Select((val, idx) => val + cashContribution[idx]);
			}

			var preTaxModel = new InvestmentModel() {
				MinModelData = minModel.ToList(),
				MaxModelData = maxModel.ToList(),
				AvgModelData = avgModel.ToList()
			};

			return preTaxModel;
		}

		public static InvestmentModel GeneratePostTaxModelDefaultAfterTaxVehicle(OptionsDict options,
			List<InvestmentModel> models,
			List<decimal>? cashContribution = null) {
			//We are basically using the pretax model and applying tax to understand value after tax.
			var preTaxModel =
				GeneratePreTaxModelDefaultAfterTaxVehicle(options, models, cashContribution);

			var taxPercentage = decimal.Parse(options["VehicleTaxPercentage"]);
			var postTaxModel = new InvestmentModel() {
				MinModelData = preTaxModel.MinModelData.Select(v => v * (1 - taxPercentage))
					.ToList(),
				MaxModelData = preTaxModel.MaxModelData.Select(v => v * (1 - taxPercentage))
					.ToList(),
				AvgModelData = preTaxModel.AvgModelData.Select(v => v * (1 - taxPercentage))
					.ToList()
			};
			return postTaxModel;
		}

		public static InvestmentModel GeneratePreTaxModelDefaultPreTaxVehicle(OptionsDict options,
			List<InvestmentModel> models,
			List<decimal>? cashContribution = null) {

			var taxPercentage = decimal.Parse(options["VehicleTaxPercentage"]);
			//The logic here is a bit confusing at first, but here is an explanation of the transforms
			var minModel = Enumerable.Range(0, models[0].MinModelData.Count)                //Project the total length of the model with indexes
									.Select(model =>                                        //For each index,
										models.Select(m => m.MinModelData[model]).Sum())    //project each model's value at that index and sum
									.Select(v => v * (1 - taxPercentage));                  // Apply Taxes
			var maxModel = Enumerable.Range(0, models[0].MaxModelData.Count)
									.Select(model =>
										models.Select(m => m.MaxModelData[model]).Sum())
									.Select(v => v * (1 - taxPercentage));
			var avgModel = Enumerable.Range(0, models[0].AvgModelData.Count)
									.Select(model =>
										models.Select(m => m.AvgModelData[model]).Sum())
									.Select(v => v * (1 - taxPercentage));

			//If cash-based contributions exist, transform all models to include them
			//This version assumes that cash contributions are already taxed
			if (cashContribution != null) {
				minModel = minModel.Select((val, idx) => val + cashContribution[idx]);
				maxModel = maxModel.Select((val, idx) => val + cashContribution[idx]);
				avgModel = avgModel.Select((val, idx) => val + cashContribution[idx]);
			}

			var preTaxModel = new InvestmentModel() {
				MinModelData = minModel.ToList(),
				MaxModelData = maxModel.ToList(),
				AvgModelData = avgModel.ToList()
			};

			return preTaxModel;
		}

		public List<decimal> SimulateCashContributionsDefaultAfterTax(OptionsDict options) {
			var analysisLength = int.Parse(options["AnalysisLength"]);
			var cashContribution = decimal.Parse(options["CashContribution"]);
			var currentHoldings = CashHoldings;

			return Enumerable.Range(0, analysisLength)
				.Select((val, idx) => currentHoldings + cashContribution * idx)
				.ToList();
		}

		public List<decimal> SimulateCashContributionsDefaultPreTax(OptionsDict options) {
			var analysisLength = int.Parse(options["AnalysisLength"]);
			var cashContribution = decimal.Parse(options["CashContribution"]);
			var taxPercentage = decimal.Parse(options["VehicleTaxPercentage"]);
			var currentHoldings = CashHoldings;

			return Enumerable.Range(0, analysisLength)
				.Select((val, idx) => cashContribution * idx)
				.Select(val => val * (1 - taxPercentage))
				.Select(val => val + currentHoldings)
				.ToList();
		}

	}

	public class InvestmentVehicleBaseConfiguration : IEntityTypeConfiguration<InvestmentVehicleBase> {
		static readonly JsonSerializerOptions options = new() {
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
				.OnDelete(DeleteBehavior.Cascade);

			builder.Property(i => i.LastUpdated)
				.HasColumnType("datetime2(7)");

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