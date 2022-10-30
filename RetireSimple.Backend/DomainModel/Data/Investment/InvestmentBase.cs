using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace RetireSimple.Backend.DomainModel.Data.Investment {
	//
	public delegate InvestmentModel AnalysisDelegate<T>(T investment) where T : InvestmentBase;


	[Table("Investments")]
	public abstract class InvestmentBase {

		public int InvestmentId { get; set; }

		public string InvestmentType { get; set; }

		//This is the easiest way to store data while maintaining type safety
		//It's recommended to create getter/setter methods for properties you expect to exist in this map
		public Dictionary<string, string> InvestmentData { get; set; } = new Dictionary<string, string>();

		public string? AnalysisType { get; set; }

		public InvestmentModel InvestmentModel { get; set; }

		//public int PortfolioId { get; set; }
		//public Portfolio Portfolio { get; set; }

		//NOTE This is also useable after 
		public abstract void ResolveAnalysisDelegate(string analysisType);
		public abstract InvestmentModel InvokeAnalysis();

	}

	public class InvestmentBaseConfiguration : IEntityTypeConfiguration<InvestmentBase> {
		static JsonSerializerOptions options = new JsonSerializerOptions {
			AllowTrailingCommas = true,
			DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
			IncludeFields = true
		};

		public void Configure(EntityTypeBuilder<InvestmentBase> builder) {
			builder.HasKey(i => i.InvestmentId);

			builder.HasOne(i => i.InvestmentModel).WithOne(i => i.Investment).OnDelete(DeleteBehavior.Restrict);

			builder.HasDiscriminator(i => i.InvestmentType).HasValue<StockInvestment>("StockInvestment");

			builder.Property(i => i.InvestmentData)
				.HasConversion(
					v => JsonSerializer.Serialize(v, options),
					v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, options) ?? new Dictionary<string, string>()
				)
				.Metadata.SetValueComparer(new ValueComparer<Dictionary<string, string>>(
					(c1, c2) => c1.SequenceEqual(c2),
					c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
					c => c.ToDictionary(entry => entry.Key, entry => entry.Value)
				));

			builder.Property(i => i.AnalysisType).HasColumnName("AnalysisType");

		}
	}
}