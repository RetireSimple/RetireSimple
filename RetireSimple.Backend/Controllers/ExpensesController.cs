using Microsoft.AspNetCore.Mvc;

using RetireSimple.Engine.Api;
using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Base;

namespace RetireSimple.Backend.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class ExpensesController : ControllerBase {
		private readonly ExpensesApi _api;

		public ExpensesController(EngineDbContext context) {
			_api = new ExpensesApi(context);
		}

		[HttpGet]
		[Route("{id:int}")]
		public ActionResult<List<Expense>> GetExpense(int id) {
			return Ok(_api.GetExpenses(id));
		}


		[HttpPost]
		public ActionResult AddExpense([FromBody] OptionsDict expense) {
			var data = new OptionsDict(expense);
			var investmentId = int.Parse(data["sourceInvestmentId"]);
			data.Remove("sourceInvestmentId");
			data.Remove("expenseId");

			_api.Add(investmentId, data);

			return Ok();
		}

		[HttpDelete]
		[Route("{id:int}")]
		public ActionResult DeleteExpense(int id) {
			_api.Remove(id);

			return Ok();
		}

	}
}