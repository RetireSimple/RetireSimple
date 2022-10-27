using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RetireSimple.Backend.DomainModel.Data.Investment;
using System.Text.Json;

namespace RetireSimple.Backend.DomainModel.Data {
	public class InvestmentModel {
		public int InvestmentModelId { get; set; }
		//TODO add more statistics fields when we get there

		//TODO ensure relationship to InvestmentBase
		public int InvestmentId { get; set; }
		public InvestmentBase Investment { get; set; }


		//TODO change to Math.NET/other types if needed
		public List<(double, double)> MaxModelData { get; set; } = new List<(double, double)>();
		public List<(double, double)> MinModelData { get; set; } = new List<(double, double)>();
	}

	public class InvestmentModelConfiguration : IEntityTypeConfiguration<InvestmentModel> {
		static JsonSerializerOptions options = new JsonSerializerOptions {
			AllowTrailingCommas = true,
			DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
		};
		public void Configure(EntityTypeBuilder<InvestmentModel> builder) {
			builder.ToTable("InvestmentModel");
			builder.HasKey(i => new { i.InvestmentModelId, i.InvestmentId });
			builder.HasOne(i => i.Investment).WithOne().HasForeignKey<InvestmentModel>(i => i.InvestmentId).IsRequired(true);

			builder.Property(i => i.MaxModelData)
			.HasConversion(
				v => JsonSerializer.Serialize(v, options),
				v => JsonSerializer.Deserialize<List<(double, double)>>(v, options) ?? new List<(double, double)>()
			)
			.Metadata.SetValueComparer(new ValueComparer<List<(double, double)>>(
				(c1, c2) => c1.SequenceEqual(c2),
				c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
				c => c.ToList())
			);

			builder.Property(i => i.MinModelData)
			.HasConversion(
				v => JsonSerializer.Serialize(v, options),
				v => JsonSerializer.Deserialize<List<(double, double)>>(v, options) ?? new List<(double, double)>()
			)
			.Metadata.SetValueComparer(new ValueComparer<List<(double, double)>>(
				(c1, c2) => c1.SequenceEqual(c2),
				c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
				c => c.ToList())
			);
		}
	}

}
