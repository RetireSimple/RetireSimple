using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RetireSimple.Backend.DomainModel.Data.Expense;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RetireSimple.Backend.DomainModel.Data.Investment {
	//
	public delegate InvestmentModel AnalysisDelegate<T>(T investment, OptionsDict options) where T : InvestmentBase;


	[Table("Investments")]
	public abstract class InvestmentBase {

		public int InvestmentId { get; set; }

		public string InvestmentType { get; set; }

		//This is the easiest way to store data while maintaining type safety
		//It's recommended to create getter/setter methods for properties you expect to exist in this map
		public OptionsDict InvestmentData { get; set; } = new OptionsDict();

		public OptionsDict AnalysisOptionsOverrides { get; set; } = new OptionsDict();

		public string? AnalysisType { get; set; }

		[JsonIgnore]
		public List<ExpenseBase> Expenses { get; set; } = new List<ExpenseBase>();

		[JsonIgnore]
		public List<InvestmentTransfer> TransfersFrom { get; set; } = new List<InvestmentTransfer>();

		[JsonIgnore]
		public List<InvestmentTransfer> TransfersTo { get; set; } = new List<InvestmentTransfer>();

		public DateTime? LastAnalysis { get; set; }

		[JsonIgnore]
		public InvestmentModel? InvestmentModel { get; set; }


		public int PortfolioId { get; set; }
		//public Portfolio Portfolio { get; set; }

		//NOTE This is also useable after 
		public abstract void ResolveAnalysisDelegate(string analysisType);
		public abstract InvestmentModel InvokeAnalysis(OptionsDict options);

	}

	public class InvestmentBaseConfiguration : IEntityTypeConfiguration<InvestmentBase> {
		static JsonSerializerOptions options = new JsonSerializerOptions {
			AllowTrailingCommas = true,
			DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
			IncludeFields = true
		};

		public void Configure(EntityTypeBuilder<InvestmentBase> builder) {
			builder.HasKey(i => i.InvestmentId);

			//TODO Allow Cascade of models as those should not exist if the investment still exists
			builder.HasOne(i => i.InvestmentModel)
					.WithOne(i => i.Investment)
					.OnDelete(DeleteBehavior.Restrict);

			builder.HasDiscriminator(i => i.InvestmentType)
					.HasValue<StockInvestment>("StockInvestment")
					.HasValue<BondInvestment>("BondInvestment")
					.HasValue<FixedInvestment>("FixedInvestment")
					.HasValue<CashInvestment>("CashInvestment")
					.HasValue<SocialSecurityInvestment>("SocialSecurityInvestment")
					.HasValue<AnnuityInvestment>("AnnuityInvestment")
					.HasValue<PensionInvestment>("PensionInvestment");

			builder.Property(i => i.InvestmentData)
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

			builder.Property(i => i.AnalysisType)
					.HasColumnName("AnalysisType");

			builder.Property(i => i.LastAnalysis)
					.HasColumnType("datetime2(7)");


		}
	}
}