using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Analysis;

namespace RetireSimple.Engine.Analysis.Utils {
	public static class ExpenseUtils {

		internal static int MonthsFromToday(DateOnly date) {
			var today = DateOnly.FromDateTime(DateTime.Now);
			return (date.Year - today.Year) * 12 + (date.Month - today.Month);
		}

		internal static List<decimal> ProjectExpenses(EngineDbContext context, int investmentId, int length) {
			var expenses = context.Expense.Where(e => e.SourceInvestmentId == investmentId).ToList();
			//God bless C# Collections/LINQ
			var expensePairs = expenses.SelectMany(e => e.GetExpenseDates().Select(Date => (Date, e.Amount)))
										.Select(e => (Date: MonthsFromToday(e.Date), e.Amount))
										.Aggregate(new Dictionary<int, decimal>(), (acc, e) => {
											if (acc.Count == 0) {
												acc.Add(e.Date, e.Amount);
												return acc;
											}

											if (acc.ContainsKey(e.Date)) {
												acc[e.Date] += e.Amount;
											} else {
												acc[e.Date] = e.Amount;
											}
											return acc;
										});

			var expenseProjections = new List<decimal>(length);
			var totalOffset = 0M;
			for (var i = 0; i < length; i++) {
				if (expensePairs.ContainsKey(i)) {
					totalOffset += expensePairs[i];
				}
				expenseProjections.Add(totalOffset);
			}

			return expenseProjections;
		}

		/// <summary>
		/// Alters the referenced model by subtracting the expenses from the model data,
		/// if data falls below zero, it is set to zero.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="expenses"></param>
		internal static void ApplyExpenses(ref InvestmentModel model, List<decimal> expenses) {
			model.AvgModelData = model.AvgModelData.Select((price, i) => Math.Max(0, price - expenses[i])).ToList();
			model.MinModelData = model.MinModelData.Select((price, i) => Math.Max(0, price - expenses[i])).ToList();
			model.MaxModelData = model.MaxModelData.Select((price, i) => Math.Max(0, price - expenses[i])).ToList();
		}
	}
}