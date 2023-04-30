using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Base;
using RetireSimple.Engine.Data.Expense;

namespace RetireSimple.Engine.Api {
	public class ExpensesApi {

		private readonly EngineDbContext _context;

		public ExpensesApi(EngineDbContext context) {
			_context = context;
		}


		/// <summary>
		///	Adds a new expense to the investment with the specified id.
		/// The speficied expense data in <paramref name="expenseData"/>
		/// is used to create the type of expense and the requisite data
		/// needed.
		/// </summary>
		/// <param name="investmentId">The ID of the <see cref="Investment"/>
		/// associated with this investment</param>
		/// <param name="expenseData">The data the expense should have </param>
		/// <returns>The ID of the newly created expense</returns>
		/// <exception cref="ArgumentException">Thrown if no investment
		/// with the specified id exists</exception>
		public int Add(int investmentId, OptionsDict expenseData) {
			if (!_context.Investment.Any(i => i.InvestmentId == investmentId)) {
				throw new ArgumentException($"No investment with id {investmentId} exists");
			}

			var data = new OptionsDict(expenseData);
			var amount = decimal.Parse(expenseData["amount"]);
			var expenseType = expenseData["expenseType"];
			data.Remove("amount");
			data.Remove("expenseType");

			//Coerce Switch Expression Type
			Expense expense = expenseType switch {
				"Recurring" => new RecurringExpense() {
					Amount = amount,
					ExpenseData = data,
					SourceInvestmentId = investmentId
				},
				"OneTime" => new OneTimeExpense() {
					Amount = amount,
					ExpenseData = data,
					SourceInvestmentId = investmentId
				},
				_ => throw new ArgumentException($"Invalid expense type {expenseType}")
			};

			_context.Expense.Add(expense);
			_context.SaveChanges();

			return expense.ExpenseId;
		}

		/// <summary>
		/// Deletes the expense with the specified id
		/// </summary>
		/// <param name="id">The ID of the expense to remove</param>
		/// <exception cref="ArgumentException">Thrown if no expense with the specified id exists</exception>
		public void Remove(int id) {
			if (!_context.Expense.Any(e => e.ExpenseId == id)) {
				throw new ArgumentException($"No expense with id {id} exists");
			}
			var expense = _context.Expense.Find(id) ?? throw new ArgumentException($"No expense with id {id} exists");
			_context.Expense.Remove(expense);
			_context.SaveChanges();
		}


		/// <summary>
		/// Updates the values in the <see cref="Expense.ExpenseData"/> of the
		/// expense with the specified id. No actual verification is done to
		/// ensure that the data is valid for the type of expense.
		/// </summary>
		/// <param name="expenseId">The ID of the expense object to updates</param>
		/// <param name="expenseData">The data to update</param>
		/// <exception cref="ArgumentException">Thrown if no expense with the specified id exists</exception>
		public void Update(int expenseId, OptionsDict expenseData) {
			if (!_context.Expense.Any(e => e.ExpenseId == expenseId)) {
				throw new ArgumentException($"No expense with id {expenseId} exists");
			}

			var expense = _context.Expense.Find(expenseId) ?? throw new ArgumentException($"No expense with id {expenseId} exists");
			var updates = new OptionsDict(expenseData);

			if (updates.ContainsKey("amount")) {
				expense.Amount = decimal.Parse(updates["amount"]);
				updates.Remove("amount");
			}

			foreach (var key in updates.Keys) {
				expense.ExpenseData[key] = updates[key];
			}
			_context.SaveChanges();
		}

		/// <summary>
		///Returns a list of all expenses associated with the investment with the specified id
		/// </summary>
		/// <param name="investmentId"></param>
		/// <returns>A List of all <see cref="Expense"/> objects associated with the investment</returns>
		public List<Expense> GetExpenses(int investmentId) {
			if (!_context.Investment.Any(i => i.InvestmentId == investmentId)) {
				throw new ArgumentException($"No investment with id {investmentId} exists");
			}

			return _context.Expense.Where(e => e.SourceInvestmentId == investmentId).ToList();
		}

	}
}

