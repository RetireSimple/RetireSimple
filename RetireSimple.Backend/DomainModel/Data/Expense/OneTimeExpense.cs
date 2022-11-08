namespace RetireSimple.Backend.DomainModel.Data.Expense {
	public class OneTimeExpense : ExpenseBase {
		public DateTime Date { get; set; }

		public override List<DateTime> GetExpenseDates() => new List<DateTime> { this.Date };


	}

}
