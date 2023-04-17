using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using RetireSimple.Engine.Api;
using RetireSimple.Engine.Data;

namespace RetireSimple.Backend.Controllers {

	[ApiController]
	[Route("api/[controller]")]
	public class AnalysisController : ControllerBase {
		private readonly InvestmentApi _investmentApi;
		private readonly PortfolioApi _portfolioApi;
		private readonly InvestmentVehicleApi _vehicleApi;

		public AnalysisController(EngineDbContext context) {
			_investmentApi = new(context);
			_portfolioApi = new(context);
			_vehicleApi = new(context);
		}

		[HttpPost]
		[Route("Investment/{id}")]
		public ActionResult GetAnalysis(int id, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] OptionsDict? options) {
			if (options is null || options.Count == 0) options = null;
			var model = _investmentApi.GetAnalysis(id, options);
			return Ok(model);
		}

		[HttpPost]
		[Route("Investment")]
		public ActionResult GetAllInvestmentModels() {
			var models = _investmentApi.GetAllAnalysis();
			return Ok(models);
		}

		[HttpPost]
		[Route("Vehicle/{id}")]
		public ActionResult GetVehicleAnalysis(int id, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] OptionsDict? options) {
			if (options is null || options.Count == 0) options = null;
			var model = _vehicleApi.GetAnalysis(id, options);
			return Ok(model);
		}


		[HttpPost]
		[Route("Portfolio")]
		public ActionResult GetPortfolioAnalysis() {
			var analysis = _portfolioApi.GetAnalysis(1, null);
			return Ok(analysis);
		}

		[HttpGet]
		[Route("Presets")]
		public ActionResult GetPresets() {

			return Ok(ReflectionUtils.GetAllPresets());
		}
	}
}
