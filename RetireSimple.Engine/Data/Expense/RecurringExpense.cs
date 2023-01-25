namespace RetireSimple.Engine.Data.Expense
{
    /// <summary>
    /// Represent a recurring expense on an investment
    /// </summary>
    public class RecurringExpense : ExpenseBase
    {
        /// <summary>
        /// How many months between each expense
        /// (e.g. 1 = monthly, 3 = quarterly, 12 = yearly)
        /// </summary>
        public int Frequency { get; set; }

        /// <summary>
        /// The date of the first recurrence
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The date of the last recurrence
        /// </summary>
        public DateTime? EndDate { get; set; }

        public override List<DateTime> GetExpenseDates() => throw new NotImplementedException();
    }
}
