using Microsoft.Extensions.Options;
using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Investment;

namespace RetireSimple.Backend.DomainModel.Analysis {

	public class StockAS {

		//This dictionary is statically available to allow for a common set of defaults that all
		//analysis modules for the same type of investment can use. 

		public static readonly OptionsDict DefaultStockAnalysisOptions = new() {
			["AnalysisLength"] = "60",                          //Number of months to project
			["StockAnalysisExpectedGrowth"] = "0.1",            //Expected Percentage Growth of the stock

		};

		#region "Dividend Calculations"
		/// NOTE: These do not produce <see cref="InvestmentModel"/> objects. They are used in conjunction with an existing model 
		/// in most cases to calculate the resulting stock value

		private static int GetDividendIntervalMonths(string interval) => interval switch {
			"Month" => 1,
			"Quarter" => 3,
			"Annual" => 12,
			_ => throw new ArgumentException("Invalid Dividend Interval")
		};

		/// <summary>
		/// Generates a list of monthly projected stock quantities using stock dividend distribution.
		/// First element of list is assumed to be for the current month/year.
		/// </summary>
		/// <param name="investment"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static List<decimal> ProjectStockDividend(StockInvestment investment, OptionsDict options) {
			var quantityList = new List<decimal>();
			var stockQuantity = investment.StockQuantity;
			var dividendPercent = investment.StockDividendPercent;
			var currentMonth = DateTime.Now.Month;
			var firstDividendMonth = investment.StockDividendFirstPaymentDate.Month;
			var monthInterval = GetDividendIntervalMonths(investment.StockDividendDistributionInterval);

			//quantityList.Add(stockQuantity);
			for(int i = 0; i < int.Parse(options["AnalysisLength"]); i++) {
				if((currentMonth - firstDividendMonth) % monthInterval == 0) {
					stockQuantity += stockQuantity * dividendPercent;
				}
				quantityList.Add(stockQuantity);
				currentMonth++;
				if(currentMonth > 12) {
					currentMonth = 1;
				}
			}

			return quantityList;
		}


		#endregion

		//TODO Move to Testing/Debugging
		public static InvestmentModel testAnalysis(StockInvestment investment, OptionsDict options) {
			var value = investment.StockPrice * investment.StockQuantity;

			//HACK I think this code is leaky, but it won't exist in final builds
			//Just PoC for Investments

			return new InvestmentModel() {
				InvestmentId = investment.InvestmentId,
				MaxModelData = new List<(decimal, decimal)>() {
					(0, value),
					(1, 2*value),
					(2, 4*value)
				},
				MinModelData = new List<(decimal, decimal)>() {
					(0, value),
					(1, new decimal(0.5) * value),
					(2, new decimal(0.25) * value)
				}
			};
		}

		//TODO Move to Testing/Debugging
		public static InvestmentModel testAnalysis2(StockInvestment investment, OptionsDict options) {
			var value = investment.StockPrice * investment.StockQuantity;

			return new InvestmentModel() {
				InvestmentId = investment.InvestmentId,
				MaxModelData = new List<(decimal, decimal)>() {
					(0, value),
					(1, new decimal(1.5)*value),
					(2, 2*value)
				},
				MinModelData = new List<(decimal, decimal)>() {
					(0, value),
					(1, new decimal(0.75) * value),
					(2, new decimal(0.5) * value)
				}
			};
		}

	}
}