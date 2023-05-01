using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RetireSimple.Engine.Analysis.Utils;
using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Expense;
using RetireSimple.Engine.Data.User;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.Data.Base {
	public delegate InvestmentModel AnalysisModule<in T>(T investment, OptionsDict options) where T : Investment;


	[Table("Investments")]
	public abstract class Investment {

		/// <summary>
		/// Primary Key of the Investment Table
		/// </summary>
		public int InvestmentId { get; set; }

		/// <summary>
		/// Name of the investment (e.g. "My 401k")
		/// </summary>
		public string InvestmentName { get; set; } = "";

		/// <summary>
		/// Discriminator Field for the Investment Table to differentiate investment Types.
		/// Check the discriminator configuration in <see cref="InvestmentBaseConfiguration"/> for valid discriminator values.
		/// </summary>
		public string InvestmentType { get; set; }

		/// <summary>
		/// This is the easiest way to store data while maintaining type safety.<br/>
		/// It's recommended to create getter/setter methods for properties you expect to exist in this map
		/// </summary>
		public OptionsDict InvestmentData { get; set; } = new OptionsDict();

		/// <summary>
		/// Overrides to pass to the analysis module when invoking analysis. <br/>
		/// Check the summary of the specified analysis module for the possible analysis keys it expects. It is also important to note that the analysis module may not use all of the keys you pass in.
		/// </summary>
		public OptionsDict AnalysisOptionsOverrides { get; set; } = new OptionsDict();

		/// <summary>
		/// The type of analysis module to use when invoking analysis. <br/>
		/// </summary>
		public string? AnalysisType { get; set; }

		/// <summary>
		/// List of expense objects associated with this investment. <br/>
		/// This is not passed during JSON Serialization and should be accessed through EF.
		/// </summary>
		[JsonIgnore]
		public List<Expense> Expenses { get; set; } = new List<Expense>();

		/// <summary>
		/// Value of the last time the object was updated. Can be used with <see cref="InvestmentModel.LastUpdated"/> to determine if analysis needs to be run again.
		/// </summary>
		public DateTime? LastUpdated { get; set; }

		/// <summary>
		/// The created <see cref="InvestmentModel"/> for this investment based on the <see cref="AnalysisModule{T}"/> run. If analysis has not been run prevously, this will be null.
		/// </summary>
		[JsonIgnore]
		public InvestmentModel? InvestmentModel { get; set; }

		/// <summary>
		/// The ID of the <see cref="Portfolio"/> that contains this investment object.
		/// </summary>
		public int PortfolioId { get; set; }


		/// <summary>
		/// Stub for invoking an associated analysis module. <br/>
		/// There is a bunch of things you can do before actually invoking the delegate, such as applying overrides or other logic. <br/>
		/// </summary>
		/// <param name="options">A set of externally defined options that should be used as an override</param>
		/// <returns>Specifc model generated by the analysis module</returns>
		public abstract InvestmentModel InvokeAnalysis(OptionsDict options);

		/// <summary>
		///	Template Method to "post-process" models after invoking analysis. <br/>
		/// Post-Processing includes things like applying expenses, zero floors, etc. <br/>
		/// This method can be overridden to provide custom post-processing logic, but is discouraged. <br/>
		/// </summary>
		/// <param name="options">Options to pass into analysis </param>
		/// <returns></returns>
		public virtual InvestmentModel PerformAnalysis(OptionsDict options) {
			var model = InvokeAnalysis(options);
			var projectedExpenses = ExpenseUtils.ProjectExpenses(this, model.AvgModelData.Count);
			ExpenseUtils.ApplyExpenses(ref model, projectedExpenses);

			//Apply Zero Floor
			model.AvgModelData = model.AvgModelData.Select(price => Math.Max(0, price)).ToList();
			model.MinModelData = model.MinModelData.Select(price => Math.Max(0, price)).ToList();
			model.MaxModelData = model.MaxModelData.Select(price => Math.Max(0, price)).ToList();

			return model;
		}

		protected Investment(string analysisType) {
			//Get the derived type for the investment type property
			InvestmentType = GetType().Name;
			AnalysisType = analysisType;
			ResolveAnalysisDelegate();
		}

		public void ResolveAnalysisDelegate() {
			if (AnalysisType == null || AnalysisType == "") {
				ReflectionUtils.SetAnalysisModuleDelegate(this, null);
				return;
			}
			var type = GetType().Name;
			var moduleList = ReflectionUtils.GetAnalysisModules(type);
			if (!moduleList.ContainsKey(AnalysisType)) {
				throw new ArgumentException($"Analysis Module {AnalysisType} does not exist for {GetType().Name}");
			}

			//Ensure we are invoking the correct generic version
			var setDelegateMethod = typeof(ReflectionUtils).GetMethod("SetAnalysisModuleDelegate")?.MakeGenericMethod(GetType());
			setDelegateMethod?.Invoke(null, new object[] { this, moduleList[AnalysisType] });
		}
	}


	public class InvestmentBaseConfiguration : IEntityTypeConfiguration<Investment> {
		static readonly JsonSerializerOptions options = new() {
			AllowTrailingCommas = true,
			DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
			IncludeFields = true
		};

		public void Configure(EntityTypeBuilder<Investment> builder) {
			builder.HasKey(i => i.InvestmentId);

			//TODO Allow Cascade of models as those should not exist if the investment still exists
			builder.HasOne(i => i.InvestmentModel)
					.WithOne(i => i.Investment)
					.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(i => i.Expenses)
				.WithOne(i => i.SourceInvestment)
				.OnDelete(DeleteBehavior.Cascade);

			//Discriminator Configuration via Reflection
			var investmentModules = ReflectionUtils.GetInvestmentModules();
			var discriminatorBuilder = builder.HasDiscriminator(i => i.InvestmentType);
			foreach (var module in investmentModules) {
				discriminatorBuilder.HasValue(module, module.Name);
			}

#pragma warning disable CS8604 // Possible null reference argument.
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
#pragma warning restore CS8604 // Possible null reference argument.

			builder.Property(i => i.InvestmentName)
				.HasDefaultValue("");

			builder.Property(i => i.AnalysisType)
					.HasColumnName("AnalysisType");

			builder.Property(i => i.LastUpdated)
					.HasColumnType("datetime2(7)");

			builder.Navigation(i => i.InvestmentModel).AutoInclude();

			builder.Navigation(i => i.Expenses).AutoInclude();

		}
	}
}