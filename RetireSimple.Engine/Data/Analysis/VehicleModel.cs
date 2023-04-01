using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RetireSimple.Engine.Data.Base;

using System.Text.Json;

namespace RetireSimple.Engine.Data.Analysis {
	public class VehicleModel {

		public int ModelId { get; set; }
		public int InvestmentVehicleId { get; set; }

		public DateTime LastUpdated { get; set; }

		//Index defaults to the number of months since model start point
		//Assertion: All lists are of the same length otherwise calcs get screwy
		public List<decimal> MaxModelData { get; set; } = new List<decimal>();
		public List<decimal> MinModelData { get; set; } = new List<decimal>();
		public List<decimal> AvgModelData { get; set; } = new List<decimal>();

		//Additional model that calculates the value of the vechile after applying taxes on withdrawals
		//Won't Apply to certain vehicles like Roth IRA, but will apply to 401k, Traditional IRA, etc
		public List<decimal> TaxDeductedMaxModelData { get; set; } = new List<decimal>();
		public List<decimal> TaxDeductedMinModelData { get; set; } = new List<decimal>();
		public List<decimal> TaxDeductedAvgModelData { get; set; } = new List<decimal>();


		/// <summary>
		/// Parameterless Constructor for EF, DO NOT REMOVE
		/// </summary>
		public VehicleModel() { }

		public VehicleModel(int vehicleId, InvestmentModel preTaxModel, InvestmentModel postTaxModel) {
			InvestmentVehicleId = vehicleId;
			LastUpdated = DateTime.Now;

			MaxModelData = preTaxModel.MaxModelData;
			MinModelData = preTaxModel.MinModelData;
			AvgModelData = preTaxModel.AvgModelData;

			TaxDeductedMaxModelData = postTaxModel.MaxModelData;
			TaxDeductedMinModelData = postTaxModel.MinModelData;
			TaxDeductedAvgModelData = postTaxModel.AvgModelData;
		}

		//Methods to produce statistical information per step/overall of the model
	}

	public class InvestmentVehicleModelConfiguration : IEntityTypeConfiguration<VehicleModel> {
		static readonly JsonSerializerOptions options = new() {
			AllowTrailingCommas = true,
			DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
			IncludeFields = true
		};

		public void Configure(EntityTypeBuilder<VehicleModel> builder) {
			builder.HasKey(i => i.ModelId);

			builder.Property(i => i.LastUpdated)
		.HasColumnType("datetime2");

			builder.HasOne<Base.InvestmentVehicle>()
				.WithOne(i => i.InvestmentVehicleModel)
				.HasForeignKey<VehicleModel>(i => i.InvestmentVehicleId)
				.IsRequired(true);

#pragma warning disable CS8604 // Possible null reference argument.
			builder.Property(i => i.MaxModelData)
			.HasConversion(
					v => JsonSerializer.Serialize(v, options),
					v => JsonSerializer.Deserialize<List<decimal>>(v, options) ?? new List<decimal>()
			).Metadata.SetValueComparer(new ValueComparer<List<decimal>>(
					(c1, c2) => c1.SequenceEqual(c2),
					c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
					c => c.ToList())
			);

			builder.Property(i => i.MinModelData)
			.HasConversion(
				v => JsonSerializer.Serialize(v, options),
				v => JsonSerializer.Deserialize<List<decimal>>(v, options) ?? new List<decimal>()
			).Metadata.SetValueComparer(new ValueComparer<List<decimal>>(
				(c1, c2) => c1.SequenceEqual(c2),
				c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
				c => c.ToList())
			);

			builder.Property(i => i.AvgModelData)
			.HasConversion(
				v => JsonSerializer.Serialize(v, options),
				v => JsonSerializer.Deserialize<List<decimal>>(v, options) ?? new List<decimal>()
			).Metadata.SetValueComparer(new ValueComparer<List<decimal>>(
				(c1, c2) => c1.SequenceEqual(c2),
				c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
				c => c.ToList())
			);

			builder.Property(i => i.TaxDeductedMaxModelData)
			.HasConversion(
				v => JsonSerializer.Serialize(v, options),
				v => JsonSerializer.Deserialize<List<decimal>>(v, options) ?? new List<decimal>()
			).Metadata.SetValueComparer(new ValueComparer<List<decimal>>(
				(c1, c2) => c1.SequenceEqual(c2),
				c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
				c => c.ToList())
			);

			builder.Property(i => i.TaxDeductedMinModelData)
			.HasConversion(
				v => JsonSerializer.Serialize(v, options),
				v => JsonSerializer.Deserialize<List<decimal>>(v, options) ?? new List<decimal>()
			).Metadata.SetValueComparer(new ValueComparer<List<decimal>>(
				(c1, c2) => c1.SequenceEqual(c2),
				c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
				c => c.ToList())
			);

			builder.Property(i => i.TaxDeductedAvgModelData)
			.HasConversion(
				v => JsonSerializer.Serialize(v, options),
				v => JsonSerializer.Deserialize<List<decimal>>(v, options) ?? new List<decimal>()
			).Metadata.SetValueComparer(new ValueComparer<List<decimal>>(
				(c1, c2) => c1.SequenceEqual(c2),
				c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
				c => c.ToList())
			);
#pragma warning restore CS8604 // Possible null reference argument.

		}
	}
}
