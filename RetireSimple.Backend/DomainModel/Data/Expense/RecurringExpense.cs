namespace RetireSimple.Backend.DomainModel.Data.Expense {
	public class RecurringExpense : ExpenseBase {
		//How many months between each expense
		//(e.g. 1 = monthly, 3 = quarterly, 12 = yearly)
		public int Frequency { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }

		public override List<DateTime> GetExpenseDates() => throw new NotImplementedException();
	}
}
