using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RetireSimple.Engine.Data.Expense;

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
		public double Amount { get; set; }

		/// <summary>
		/// Generate the dates that the expense will affect the model depending on the expense type.
		/// </summary>
		/// <returns></returns>
		public abstract List<DateTime> GetExpenseDates();
	}

	public class ExpenseBaseConfiguration : IEntityTypeConfiguration<Expense> {
		public void Configure(EntityTypeBuilder<Expense> builder) {
			builder.ToTable("Expenses");
			builder.HasKey(e => e.ExpenseId);
			builder.HasDiscriminator()
				.HasValue<OneTimeExpense>("OneTime")
				.HasValue<RecurringExpense>("Recurring");
			builder.HasOne(e => e.SourceInvestment)
				.WithMany(e => e.Expenses)
				.HasForeignKey(e => e.SourceInvestmentId)
				.IsRequired();
			builder.Property(e => e.Amount)
				.IsRequired()
				.HasDefaultValue(0);

		}
	}
}