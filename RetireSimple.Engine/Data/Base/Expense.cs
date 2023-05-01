using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RetireSimple.Engine.Data.Expense;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.Data.Base {

	/// <summary>
	/// The base class
	/// </summary>
	public abstract class Expense {
		/// <summary>
		/// Primary Key of the Expense Table
		/// </summary>
		public int ExpenseId { get; set; }

		public string ExpenseType { get; set; }

		/// <summary>
		/// Foreign Key of the <see cref="Investment"/> that this expense is associated with.
		/// </summary>
		public int SourceInvestmentId { get; set; }

		/// <summary>
		/// The <see cref="Investment"/> that this expense is associated with.
		/// </summary>
		[JsonIgnore]
		public Investment SourceInvestment { get; set; }

		/// <summary>
		/// The amount this expense deducts from the investment.
		/// </summary>
		public decimal Amount { get; set; }

		///	 <summary>
		/// Common Storage for fields that are unique to the expense type.
		/// </summary>
		/// <returns></returns>
		public OptionsDict ExpenseData { get; set; } = new OptionsDict();

		/// <summary>
		/// Generate the dates that the expense will affect the model depending on the expense type.
		/// </summary>
		/// <returns></returns>
		public abstract List<DateOnly> GetExpenseDates();
	}

	public class ExpenseBaseConfiguration : IEntityTypeConfiguration<Expense> {
		static readonly JsonSerializerOptions options = new() {
			AllowTrailingCommas = true,
			DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
			IncludeFields = true
		};

		public void Configure(EntityTypeBuilder<Expense> builder) {
			builder.ToTable("Expenses");
			builder.HasKey(e => e.ExpenseId);
			builder.HasDiscriminator(e => e.ExpenseType)
				.HasValue<OneTimeExpense>("OneTime")
				.HasValue<RecurringExpense>("Recurring");
			builder.HasOne(e => e.SourceInvestment)
				.WithMany(e => e.Expenses)
				.HasForeignKey(e => e.SourceInvestmentId)
				.IsRequired();
			builder.Property(e => e.Amount)
				.IsRequired()
				.HasDefaultValue(0);

#pragma warning disable CS8604 // Possible null reference argument.
			builder.Property(e => e.ExpenseData)
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
		}
	}
}