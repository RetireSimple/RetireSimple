using Microsoft.AspNetCore.Mvc;

using RetireSimple.Engine.Api;
using RetireSimple.Engine.Data;

namespace RetireSimple.Backend.Controllers {

	[ApiController]
	[Route("api/[controller]")]

	public class AnalysisController : ControllerBase {
		private readonly InvestmentApi _investmentApi;

		public AnalysisController(EngineDbContext context) {
			_investmentApi = new(context);
		}

		[HttpPost]
		[Route("/Investment/GetAnalysis/{id}")]
		public ActionResult GetAnalysis(int id, OptionsDict? options) {
			try {
				var model = _investmentApi.GetAnalysis(id, options);
				return Ok(model);
			}
			catch (InvalidOperationException) {
				return NotFound("Investment not found or has an unknown analysis module defined");
			}
		}

		// [HttpPost]
		// [Route("/Investment/UpdateOverrides/{id}")]

	}
}
