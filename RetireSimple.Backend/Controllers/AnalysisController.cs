﻿using Microsoft.AspNetCore.Mvc;
using RetireSimple.Engine.Data;

namespace RetireSimple.Backend.Controllers {

	[ApiController]
	[Route("api/[controller]")]

	public class AnalysisController : ControllerBase {
		private readonly EngineDbContext _context;

		public AnalysisController(EngineDbContext context) {
			_context = context;
		}

		[HttpPost]
		[Route("GetAnalysis")]
		public ActionResult GetAnalysis([FromQuery] int investmentID) {
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
