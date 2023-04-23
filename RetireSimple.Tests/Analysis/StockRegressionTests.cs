using FluentAssertions.Execution;
using MathNet.Numerics.Distributions;

using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static RetireSimple.Engine.Analysis.MonteCarlo;
using static RetireSimple.Engine.Analysis.Regression;

namespace RetireSimple.Tests.Analysis {
	public class StockRegressionTests {
		public StockInvestment TestInvestment { get; set; }

		public StockRegressionTests() {
			TestInvestment = new StockInvestment("") {
				StockPrice = 100,
				StockPurchasePrice = 95,
				StockPurchaseDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
			};
		}

		[Fact]
		public void TestRegressionSimple() {
			var options = new RegressionOptions() {
				BasePrice = TestInvestment.StockPrice,
				AnalysisLength = 60,
			};

			using (new AssertionScope("root")) {

				//Rerun test multiple times because of probability
				
				var currentPrice = TestInvestment.StockPrice;
				var purchasePrice = TestInvestment.PurchasePrice;
				int monthPurchased = TestInvestment.StockPurchaseDate.Month + TestInvestment.StockPurchaseDate.Year * 12;
				int monthCurrent = DateTime.Now.Month + DateTime.Now.Year * 12;
				//var sector = TestInvestment.Sector;
				var quantity = TestInvestment.StockQuantity;

				var analysisLength = options.AnalysisLength;
				
				decimal slope = (currentPrice - purchasePrice) / (monthCurrent - monthPurchased);
				var projectedDividend = ProjectStockDividend(TestInvestment, options);

				List<decimal> futurePrices = new List<decimal>();
				decimal intercept = currentPrice - slope * monthPurchased;
				decimal averageGrowth = slope / intercept;
				for (int i = 0; i < options.AnalysisLength; i++) {
					monthCurrent++;
					decimal price = intercept + (slope * monthCurrent);
					Assert.Equal(ListOfBondVal[22].ToString("#.##"), actual[1].ToString());
				}

			}
		}
	}
}
