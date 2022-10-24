using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RetireSimple.Backend.DomainModel.Data {
    [Table("Investments")]
    public abstract class InvestmentBase {

        [Key]
        [Column("Id", TypeName = "int")]
        [JsonIgnore]
        public int InvestmentId { get => InvestmentId; set => InvestmentId = value; }

        public string InvestmentType { get => InvestmentType; set => InvestmentType = value; }

        //This is the easiest way to store data while maintaining type safety
        public Dictionary<string, string> InvestmentData { get => InvestmentData; set => InvestmentData = value; }

        public abstract InvestmentModel GenerateAnalysis();




    }

    public class InvestmentBaseConfiguration : IEntityTypeConfiguration<InvestmentBase> {
		static JsonSerializerOptions options = new JsonSerializerOptions {
			AllowTrailingCommas = true,
			DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
		};

		public void Configure(EntityTypeBuilder<InvestmentBase> builder) {
            builder.Property(i => i.InvestmentData)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, options),
                    v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, options)
                );
        }
    }
}