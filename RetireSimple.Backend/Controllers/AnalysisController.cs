using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.DomainModel.User;
using RetireSimple.Backend.Services;
using MathNet.Numerics;
using MathNet.Numerics.Random;

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
			var investment = _context.Investments.First(i => i.InvestmentId == investmentID);
			//check has model 
			if(!_context.InvestmentModels.Any(m => investmentID == m.InvestmentId)) {
				investment.InvestmentModel = investment.InvokeAnalysis(new OptionsDict());
				_context.SaveChanges();
			}
			return Ok(_context.InvestmentModels.First(m => m.InvestmentId == investmentID));
		}
	}
}
