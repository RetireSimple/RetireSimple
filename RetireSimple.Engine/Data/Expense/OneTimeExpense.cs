using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.Data.Expense {
	/// <summary>
	/// Represents an Expense on an investment that occurs once
	/// </summary>
	public class OneTimeExpense : Base.Expense {
		/// <summary>
		/// The date the expense is applied to the investment
		/// </summary>
		[JsonIgnore, NotMapped]
		public DateOnly Date {
			get => DateOnly.Parse(ExpenseData["date"]);
			set => ExpenseData["date"] = value.ToShortDateString();
		}

		public override List<DateOnly> GetExpenseDates() => new() { Date };
	}
}