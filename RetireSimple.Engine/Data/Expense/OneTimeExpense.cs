using RetireSimple.Engine.Data.Base;

namespace RetireSimple.Engine.Data.Expense {
	/// <summary>
	/// Represents an Expense on an investment that occurs once
	/// </summary>
	public class OneTimeExpense : Base.Expense {
		/// <summary>
		/// The date the expense is applied to the investment
		/// </summary>
		public DateTime Date { get; set; }

		public override List<DateTime> GetExpenseDates() => new() { Date };


	}

}
