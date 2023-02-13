using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
		[Route("Investment/{id}")]
		public ActionResult GetAnalysis(int id, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] OptionsDict? options) {
			try {
				if (options is null || options.Count == 0) options = null;
				var model = _investmentApi.GetAnalysis(id, options);
				return Ok(model);
			}
			catch (InvalidOperationException) {
				return NotFound("Investment not found or has an unknown analysis module defined");
			}
		}

	}
}
