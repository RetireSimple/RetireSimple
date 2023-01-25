using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RetireSimple.Engine.Data.Investment;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.Data.Expense
{

    /// <summary>
    /// The base class 
    /// </summary>
    public abstract class ExpenseBase
    {
        /// <summary>
        /// Primary Key of the Expense Table
        /// </summary>
        public int ExpenseId { get; set; }

        /// <summary>
        /// Foreign Key of the <see cref="InvestmentBase"/> that this expense is associated with.
        /// </summary>
        public int SourceInvestmentId { get; set; }

        /// <summary>
        /// The <see cref="InvestmentBase"/> that this expense is associated with.
        /// </summary>
        [JsonIgnore]
        public InvestmentBase SourceInvestment { get; set; }

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

    public class ExpenseBaseConfiguration : IEntityTypeConfiguration<ExpenseBase>
    {
        public void Configure(EntityTypeBuilder<ExpenseBase> builder)
        {
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