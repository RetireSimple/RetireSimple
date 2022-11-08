using MathNet.Numerics;
using MathNet.Numerics.Random;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetireSimple.Backend.Controllers.RequestBody;
using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.Services;
using System;

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
		public async Task<ActionResult<List<InvestmentBase>>> GetInvestments() {
			var investments = await _context.Investments.ToListAsync();
			return Ok(investments);
		}

		[HttpPost]
		[Route("AddStock")]
		public async Task<ActionResult> AddStockInvestment([FromBody] StockAddRequestBody body) {
			var newInvestment = new StockInvestment("testAnalysis");
			newInvestment.StockPrice = body.Price;
			newInvestment.StockQuantity = body.Quantity;
			newInvestment.StockTicker = body.Ticker;
			newInvestment.StockPurchaseDate = body.Date;

			await _context.Investments.AddAsync(newInvestment);
			await _context.SaveChangesAsync();

			return Ok();
		}

		[HttpPost]
		[Route("AddRandomStock")]
		public async Task<ActionResult> AddRandomStockInvestment() {


			var newInvestment = new StockInvestment("testAnalysis");
			newInvestment.StockPrice = (_rand.NextDecimal() * 1000).Round(2);
			newInvestment.StockQuantity = _rand.Next(5000);
			newInvestment.StockTicker = MakeRandomTicker();
			newInvestment.StockPurchaseDate = DateTime.Today;

			await _context.Investments.AddAsync(newInvestment);
			await _context.SaveChangesAsync();

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
