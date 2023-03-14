using Microsoft.AspNetCore.Mvc;

using RetireSimple.Engine.Api;
using RetireSimple.Engine.Data;

namespace RetireSimple.Backend.Controllers{
	[Route("api/[controller]")]
	[ApiController]
	public class PortfolioController : ControllerBase{
		private readonly PortfolioApi _portfolioApi;
		public PortfolioController(EngineDbContext context){
			_portfolioApi = new(context);
		}
		[HttpGet]
		public ActionResult GetPortfolio(){
			var portfolio = _portfolioApi.GetPortfolio(1);
			return Ok(portfolio);
		}
	}
}