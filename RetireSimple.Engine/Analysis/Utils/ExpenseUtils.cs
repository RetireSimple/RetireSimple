// using RetireSimple.Engine.Data;

// namespace RetireSimple.Engine.Analysis.Utils {
// 	public static class ExpenseUtils {

// 		internal static int MonthsFromToday(DateOnly date) {
// 			var today = DateOnly.FromDateTime(DateTime.Now);
// 			return (date.Year - today.Year) * 12 + (date.Month - today.Month);
// 		}
// 		internal static List<decimal> ProjectExpenses(EngineDbContext context, int investmentId, int length) {
// 			var expenses = context.Expense.Where(e => e.SourceInvestmentId == investmentId).ToList();
// 			var expensePairs = expenses.SelectMany(e => e.GetExpenseDates().Select(d => (d, e.Amount)))
// 										.Select(e => (MonthsFromToday(e.d), e.Amount))
// 										.Aggregate(new Dictionary<int, decimal>(), (acc, e) => {
// 											if (acc.Count == 0) {
// 												acc.Add(e.Item1, e.Item2);
// 												return acc;
// 											}

// 											if (acc.Any(e.Item1)) {
// 												acc[^1] = (last.Item1, last.Item2 + e.Item2);
// 											} else {
// 												acc.Add((e.Item1, e.Item2));
// 											}

// 											return acc;
// 										})
// 										;


// 			var expenseProjections = new List<decimal>(length);
// 		}
// 	}
// }