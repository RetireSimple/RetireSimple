using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RetireSimple.Backend.DomainModel.Data {
	[Table("Investments")]
	public abstract class InvestmentBase {

		public int InvestmentId { get; set; }

		public string InvestmentType { get; set; }

		//This is the easiest way to store data while maintaining type safety
		//It's recommended to create getter/setter methods for properties you expect to exist in this map
		public Dictionary<string, string> InvestmentData { get; set; }

		public string AnalysisType { get; set; }
		public abstract void ResolveAnalysisDelegate(string analysisType);

		public delegate InvestmentModel GenerateAnalysis(Dictionary<string, string> data);


		//TODO use on entity create to ensure data integrity?
		public abstract void ValidateData();


	}

	public class InvestmentBaseConfiguration : IEntityTypeConfiguration<InvestmentBase> {
		static JsonSerializerOptions options = new JsonSerializerOptions {
			AllowTrailingCommas = true,
			DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
		};

		public void Configure(EntityTypeBuilder<InvestmentBase> builder) {
			builder.HasKey(i => i.InvestmentId);
			
			builder.HasDiscriminator(i => i.InvestmentType);

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
		}
	}
}