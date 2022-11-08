using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RetireSimple.Backend.DomainModel.Data.Investment;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RetireSimple.Backend.DomainModel.Data {
	public class InvestmentModel {
		public int InvestmentModelId { get; set; }
		//TODO add more statistics fields when we get there

		//TODO ensure relationship to InvestmentBase
		public int InvestmentId { get; set; }
		
		[JsonIgnore]
		public InvestmentBase Investment { get; set; }


		//TODO change to Math.NET/other types if needed
		public List<(decimal, decimal)> MaxModelData { get; set; } = new List<(decimal, decimal)>();
		public List<(decimal, decimal)> MinModelData { get; set; } = new List<(decimal, decimal)>();

		public DateTime LastUpdated { get; set; } = DateTime.Now;

		public void AddOtherData(InvestmentModel otherModel) {

			//TODO add more data fields when we get there
			MaxModelData.AddRange(otherModel.MaxModelData);
			MinModelData.AddRange(otherModel.MinModelData);
		}
	}

	public class InvestmentModelConfiguration : IEntityTypeConfiguration<InvestmentModel> {
		static JsonSerializerOptions options = new JsonSerializerOptions {
			AllowTrailingCommas = true,
			DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
			IncludeFields = true,
		};
		public void Configure(EntityTypeBuilder<InvestmentModel> builder) {
			builder.ToTable("InvestmentModel");
			builder.HasKey(i => new { i.InvestmentModelId, i.InvestmentId });
			builder.HasOne(i => i.Investment)
					.WithOne(i => i.InvestmentModel)
					.HasForeignKey<InvestmentModel>(i => i.InvestmentId)
					.IsRequired(true);

			builder.Property(i => i.MaxModelData)
			.HasConversion(
				v => JsonSerializer.Serialize(v, options),
				v => JsonSerializer.Deserialize<List<(decimal, decimal)>>(v, options) ?? new List<(decimal, decimal)>()
			)
			.Metadata.SetValueComparer(new ValueComparer<List<(decimal, decimal)>>(
				(c1, c2) => c1.SequenceEqual(c2),
				c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
				c => c.ToList())
			);

			builder.Property(i => i.MinModelData)
			.HasConversion(
				v => JsonSerializer.Serialize(v, options),
				v => JsonSerializer.Deserialize<List<(decimal, decimal)>>(v, options) ?? new List<(decimal, decimal)>()
			)
			.Metadata.SetValueComparer(new ValueComparer<List<(decimal, decimal)>>(
				(c1, c2) => c1.SequenceEqual(c2),
				c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
				c => c.ToList())
			);

			builder.Property(i => i.LastUpdated)
					.HasColumnType("datetime2");
		}
	}

}
