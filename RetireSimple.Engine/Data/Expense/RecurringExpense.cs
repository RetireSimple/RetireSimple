namespace RetireSimple.Engine.Data.Expense {
	/// <summary>
	/// Represent a recurring expense on an investment
	/// </summary>
	public class RecurringExpense : Base.Expense {
		/// <summary>
		/// How many months between each expense
		/// (e.g. 1 = monthly, 3 = quarterly, 12 = yearly)
		/// </summary>
		public int Frequency {
			get => int.Parse(ExpenseData["frequency"]);
			set => ExpenseData["frequency"] = value.ToString();
		}

		/// <summary>
		/// The date of the first recurrence
		/// </summary>
		public DateOnly StartDate {
			get => DateOnly.Parse(ExpenseData["startDate"]);
			set => ExpenseData["startDate"] = value.ToShortDateString();
		}

		/// <summary>
		/// The date of the last recurrence
		/// </summary>
		public DateOnly EndDate {
			get => DateOnly.Parse(ExpenseData["endDate"]);
			set => ExpenseData["endDate"] = value.ToShortDateString();
		}

		public override List<DateOnly> GetExpenseDates() {
			var dates = new List<DateOnly>();
			var date = StartDate;
			while (date <= EndDate) {
				dates.Add(date);
				date = date.AddMonths(Frequency);
			}
			return dates;
		}
	}
}
