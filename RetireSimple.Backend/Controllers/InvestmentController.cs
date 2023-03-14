using Microsoft.AspNetCore.Mvc;

using RetireSimple.Engine.Api;
using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Investment;

using System.Text.Json;

namespace RetireSimple.Backend.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class InvestmentController : ControllerBase {
		private readonly InvestmentApi _investmentApi;

		public InvestmentController(EngineDbContext context) {
			_investmentApi = new InvestmentApi(context);
		}

		[HttpGet]
		public ActionResult<List<InvestmentBase>> GetInvestments() {
			return Ok(_investmentApi.GetSingluarInvestments());
		}

		[HttpGet]
		[Route("{id}")]
		public ActionResult<InvestmentBase> GetInvestment(int id) {
			try {
				var investment = _investmentApi.GetInvestment(id);
				return Ok(investment);
			}
			catch (InvalidOperationException) {
				return NotFound("Investment not found");
			}
		}

		[HttpPost]
		public ActionResult AddInvestment([FromBody] JsonDocument requestBody) {
			var body = requestBody.Deserialize<OptionsDict>();
			if (body == null) {
				return BadRequest();
			}

			//Must have a investmentType defined, we check if type is valid in API call
			if (!body.ContainsKey("investmentType")) {
				return BadRequest("investmentType not defined");
			}

			try {
				var type = body["investmentType"];
				body.Remove("investmentType");

				var id = _investmentApi.Add(type, body);

				//Check if we got analysis parameters
				if (body.Keys.Any(k => k.StartsWith("analysis_"))) {
					var analysisOptions =
						body.Where(kvp => kvp.Key.StartsWith("analysis_"))
							.ToDictionary(kvp => kvp.Key.Remove(0, 9), kvp => kvp.Value);
					_investmentApi.UpdateAnalysisOptions(id, analysisOptions);
				}

				return Ok(id);
			}
			catch (ArgumentException) {
				return BadRequest("Specified type of investment is not supported");
			}
		}

		[HttpPost]
		[Route("{id}")]
		public ActionResult UpdateInvestment(int id, [FromBody] JsonDocument requestBody) {
			var body = requestBody.Deserialize<OptionsDict>();
			if (body == null) {
				return BadRequest();
			}

			try {
				_investmentApi.Update(id, body);

				//Check if we got analysis parameters
				if (body.Keys.Any(k => k.StartsWith("analysis_"))) {
					var analysisOptions =
						body.Where(kvp => kvp.Key.StartsWith("analysis_"))
							.ToDictionary(kvp => kvp.Key.Remove(0, 9), kvp => kvp.Value);
					_investmentApi.UpdateAnalysisOptions(id, analysisOptions);
				}

				return Ok();
			}
			catch (ArgumentException) {
				return NotFound("Investment not found");
			}
		}

		[HttpDelete]
		[Route("{id}")]
		public ActionResult DeleteInvestment(int id) {
			try {
				_investmentApi.Remove(id);
				return Ok();
			}
			catch (ArgumentException) {
				return NotFound("Investment not found");
			}
			catch (InvalidOperationException) {
				return BadRequest("Investment cannot be deleted. Check if it still has dependents");
			}
		}

	}
}
