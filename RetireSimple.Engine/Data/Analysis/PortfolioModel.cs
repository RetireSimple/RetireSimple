using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RetireSimple.Engine.Data.User;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.Data.Analysis {
	public class PortfolioModel {
		/// <summary>
		/// Primary Key ID of the object.
		/// </summary>
		public int PortfolioModelId { get; set; }

		/// <summary>
		/// Foreign Key ID of the Portfolio that this model is for.
		/// </summary>
		public int PortfolioId { get; set; }

		[JsonIgnore]
		public Portfolio Portfolio { get; set; }


		//Index defaults to the number of months since model start point
		//Assertion: All lists are of the same length otherwise calcs get screwy
		public List<decimal> MaxModelData { get; set; } = new List<decimal>();
		public List<decimal> MinModelData { get; set; } = new List<decimal>();
		public List<decimal> AvgModelData { get; set; } = new List<decimal>();

		public DateTime LastUpdated { get; set; } = DateTime.Now;


		//Methods to produce statistical information per step/overall of the model

	}

	public class PortfolioModelConfiguration : IEntityTypeConfiguration<PortfolioModel> {
		static readonly JsonSerializerOptions options = new() {
			AllowTrailingCommas = true,
			DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
			IncludeFields = true,
		};
		public void Configure(EntityTypeBuilder<PortfolioModel> builder) {
			builder.ToTable("PortfolioModel");
			builder.HasKey(i => new { i.PortfolioModelId });
			builder.HasOne(i => i.Portfolio)
					.WithOne(i => i.PortfolioModel)
					.HasForeignKey<PortfolioModel>(i => i.PortfolioId)
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

