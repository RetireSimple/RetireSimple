using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RetireSimple.Backend.DomainModel.Data.Investment;

namespace RetireSimple.Backend.DomainModel.Data.Expense {
	public abstract class ExpenseBase {
		public int ExpenseId { get; set; }
		public int SourceInvestmentId { get; set; }
		public InvestmentBase SourceInvestment { get; set; }
		public int PorfolioId { get; set; }

		public double Amount { get; set; }

		public abstract List<DateTime> GetExpenseDates();
	}

	public class ExpenseBaseConfiguration : IEntityTypeConfiguration<ExpenseBase> {
		public void Configure(EntityTypeBuilder<ExpenseBase> builder) {
			builder.ToTable("Expenses");
			builder.HasKey(e => e.ExpenseId);
			builder.HasOne(e => e.SourceInvestment)
					.WithMany(e => e.Expenses)
					.HasForeignKey(e => e.SourceInvestmentId)
					.IsRequired();
			builder.HasDiscriminator()
					.HasValue<OneTimeExpense>("OneTime")
					.HasValue<RecurringExpense>("Recurring");
			builder.Property(e => e.Amount)
					.IsRequired()
					.HasDefaultValue(0);

		}
	}
}