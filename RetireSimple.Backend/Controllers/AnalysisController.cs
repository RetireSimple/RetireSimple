using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.Services;

namespace RetireSimple.Backend.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class AnalysisController : ControllerBase {
		private readonly InvestmentDBContext _context;
		public static OptionsDict options = new OptionsDict();

		public AnalysisController(InvestmentDBContext context) {
			_context = context;
		}


		[HttpGet]
		[Route("GetAnalysis")]
		public async Task<ActionResult<InvestmentModel>> GetAnalysis([FromQuery] int InvestmentId) {
			var investment = await _context.Investment.FirstAsync(i => i.InvestmentId == InvestmentId);
			if(investment == null) {
				return NotFound();
			}
			if(investment.InvestmentModel is null
				|| investment.InvestmentModel.LastUpdated != investment.LastAnalysis) {
				await _context.InvestmentModel.AddAsync(investment.InvokeAnalysis(options));
			}


			return Ok(investment.InvestmentModel);
		}


	}
}
