using MathNet.Numerics;
using MathNet.Numerics.Random;
using Microsoft.AspNetCore.Mvc;
using RetireSimple.Backend.RequestBody;
using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Investment;
using System.Text.Json;

namespace RetireSimple.Backend.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class InvestmentController : ControllerBase {
		private readonly EngineDbContext _context;
		private readonly Random _rand;

		public InvestmentController(EngineDbContext context) {
			_context = context;
			_rand = new Random();
		}

		[HttpGet]
		[Route("GetAllInvestments")]
		public ActionResult<List<InvestmentBase>> GetInvestments() {
			var investments = _context.Investment.ToList();
			return Ok(investments); //converts to JSON
		}

		[HttpGet]
		[Route("GetInvestment/{id}")]
		public ActionResult<InvestmentBase> GetInvestment(int id) {
			var investment = _context.Investment.First(i => i.InvestmentId == id);
			return Ok(investment);
		}

		[HttpPost]
		[Route("AddStock")]
		public ActionResult AddStockInvestment([FromBody] JsonDocument reqBody) {
			var body = reqBody.Deserialize<OptionsDict>();
			if(body == null) {
				return BadRequest();
			}
			var newInvestment = new StockInvestment(body["analysisType"]) {
				InvestmentName = body["investmentName"],
				StockPrice = Decimal.Parse(body["stockPrice"]),
				StockQuantity = Decimal.Parse(body["stockQuantity"]),
				StockTicker = body["stockTicker"],
				StockPurchaseDate = DateTime.Parse(body["stockPurchaseDate"])
			};

			//TODO Don't Preset values
			newInvestment.InvestmentData["stockDividendPercent"] = body["stockDividendPercent"];
			newInvestment.InvestmentData["stockDividendDistributionInterval"] = body["stockDividendDistributionInterval"];
			newInvestment.InvestmentData["stockDividendDistributionMethod"] = body["stockDividendDistributionMethod"];
			newInvestment.InvestmentData["stockDividendFirstPaymentDate"] = body["stockDividendFirstPaymentDate"];

			newInvestment.AnalysisOptionsOverrides["analysisLength"] = body["analysisLength"];
			newInvestment.AnalysisOptionsOverrides["simCount"] = body["simCount"];
			newInvestment.AnalysisOptionsOverrides["RandomVariableMu"] = body["randomVariableMu"];
			newInvestment.AnalysisOptionsOverrides["RandomVariableSigma"] = body["randomVariableSigma"];
			newInvestment.AnalysisOptionsOverrides["RandomVariableScaleFactor"] = body["randomVariableScaleFactor"];

			var mainPortfolio = _context.Portfolio.First(p => p.PortfolioId == 1);
			mainPortfolio.Investments.Add(newInvestment);

			_context.SaveChanges();

			return Ok();
		}

		[HttpPost]
		[Route("AddRandomStock")]
		public ActionResult AddRandomStockInvestment() {
			var newInvestment = new StockInvestment("testAnalysis") {
				StockPrice = (_rand.NextDecimal() * 1000).Round(2),
				StockQuantity = _rand.Next(5000),
				StockTicker = MakeRandomTicker(),
				StockPurchaseDate = DateTime.Today
			};

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
