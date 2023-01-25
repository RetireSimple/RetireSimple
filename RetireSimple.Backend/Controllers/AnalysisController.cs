using Microsoft.AspNetCore.Mvc;
using RetireSimple.Engine.Data;

namespace RetireSimple.Backend.Controllers {
	//private readonly Random _rand;

	[ApiController]
	[Route("api/[controller]")]

	public class AnalysisController : ControllerBase {
		private readonly InvestmentDBContext _context;

		public AnalysisController(InvestmentDBContext context) {
			_context = context;
		}

		[HttpPost]
		[Route("GetAnaylsis")]
		public ActionResult GetAnaylsis([FromQuery] int investmentID) {
			var investment = _context.Investment.First(i => i.InvestmentId == investmentID);
			//check has model 
			if(!_context.InvestmentModel.Any(m => investmentID == m.InvestmentId)) {
				investment.InvestmentModel = investment.InvokeAnalysis(new OptionsDict());
				_context.SaveChanges();
			}
			return Ok(_context.InvestmentModel.First(m => m.InvestmentId == investmentID));
		}
	}
}
