using MathNet.Numerics;
using MathNet.Numerics.Random;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetireSimple.Backend.Controllers.RequestBody;
using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.DomainModel.User;
using RetireSimple.Backend.Services;

namespace RetireSimple.Backend.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class InvestmentController : ControllerBase {
		private readonly InvestmentDBContext _context;
		private readonly Random _rand;

		public InvestmentController(InvestmentDBContext context) {
			_context = context;
			_rand = new Random();
		}

		[HttpGet]
		[Route("GetAllInvestments")]
		public ActionResult<List<InvestmentBase>> GetInvestments() {

			var investments = _context.Investment.ToList();
			return Ok(investments); //converts to JSON
		}

		[HttpPost]
		[Route("AddStock")]
		public ActionResult AddStockInvestment([FromBody] StockAddRequestBody body) {
			var newInvestment = new StockInvestment(body.AnalysisType);
			newInvestment.InvestmentName = body.Name;
			newInvestment.StockPrice = body.Price;
			newInvestment.StockQuantity = body.Quantity;
			newInvestment.StockTicker = body.Ticker;
			//newInvestment.StockPurchaseDate = body.Date;

			var mainPortfolio = _context.Portfolio.First(p => p.PortfolioId == 1);
			mainPortfolio.Investments.Add(newInvestment);

			_context.SaveChanges();

			return Ok();
		}

		[HttpPost]
		[Route("AddRandomStock")]
		public ActionResult AddRandomStockInvestment() {
			var newInvestment = new StockInvestment("testAnalysis");
			newInvestment.StockPrice = (_rand.NextDecimal() * 1000).Round(2);
			newInvestment.StockQuantity = _rand.Next(5000);
			newInvestment.StockTicker = MakeRandomTicker();
			newInvestment.StockPurchaseDate = DateTime.Today;

			var mainPortfolio = _context.Portfolio.First(p => p.PortfolioId == 1);
			mainPortfolio.Investments.Add(newInvestment);
			_context.SaveChangesAsync();

			return Ok();
		}

		private string MakeRandomTicker() {
			//yoinked from stackoverflow for demo purposes
			//https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			return new string(Enumerable.Repeat(chars, 4)
				.Select(s => s[_rand.Next(s.Length)]).ToArray());
		}

	}
}
