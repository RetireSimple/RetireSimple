using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.Data.Analysis {
	public class InvestmentModel {
		/// <summary>
		/// Primary Key ID of the object.
		/// </summary>
		public int InvestmentModelId { get; set; }

		/// <summary>
		/// Foreign Key ID of the Investment that this model is for.
		/// </summary>
		public int InvestmentId { get; set; }

		[JsonIgnore]
		public Base.Investment Investment { get; set; }


		//Index defaults to the number of months since model start point
		//Assertion: All lists are of the same length otherwise calcs get screwy
		public List<decimal> MaxModelData { get; set; } = new List<decimal>();
		public List<decimal> MinModelData { get; set; } = new List<decimal>();
		public List<decimal> AvgModelData { get; set; } = new List<decimal>();

		public DateTime LastUpdated { get; set; } = DateTime.Now;


		//Methods to produce statistical information per step/overall of the model
		public static decimal Variance() {
			var variance = 0.0m;

			//TODO Implement Somehow??? Brain small atm

			return variance;
		}
	}

	public class InvestmentModelConfiguration : IEntityTypeConfiguration<InvestmentModel> {
		static readonly JsonSerializerOptions options = new() {
			AllowTrailingCommas = true,
			DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
			IncludeFields = true,
		};
		public void Configure(EntityTypeBuilder<InvestmentModel> builder) {
			builder.ToTable("InvestmentModel");
			builder.HasKey(i => new { i.InvestmentModelId });
			builder.HasOne(i => i.Investment)
					.WithOne(i => i.InvestmentModel)
					.HasForeignKey<InvestmentModel>(i => i.InvestmentId)
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
#pragma warning restore CS8604 // Possible null reference argument.

			builder.Property(i => i.LastUpdated)
					.HasColumnType("datetime2");
		}
	}

}
