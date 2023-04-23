using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using MathNet.Numerics.LinearRegression;

using Microsoft.Extensions.Logging;

using RetireSimple.Engine.Data.Investment;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static RetireSimple.Engine.Analysis.MonteCarlo;
using static System.Formats.Asn1.AsnWriter;

namespace RetireSimple.Engine.Analysis {
	public enum RegressionRV {
		LINEAR,
		//The following are currently not implemented, but are supported by Math.NET
		LOGISTIC,
		POLYNOMIAL,
		RIDGE,
		LASSO,
		QUANTILE,
		BAYESIAN,
		Bayesian,
		PCR,    //Principal Components Regression
		PLSR,   //Partial Least Squares Regression
		ENR,    //Elastic Net Regression
	}

	public class Regression {

		public readonly record struct RegressionOptions {
			public decimal CurrentPrice { get; init; }
			public decimal PurchasePrice { get; init; }
			public int AnalysisLength { get; init; }
			//public IContinuousDistribution RandomVariable { get; init; }
		}

		/// <summary>
		/// Utility Function to generate a Math.NET Continuous Distribution for use in Regression analysis.
		/// <param name="type"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>

		//internal static icontinuousdistribution createrandomvarinstance(montecarlorv type, dictionary<string, double> parameters) {
		//	return type switch {
		//		regressionrv.logistic =>
		//		_ => throw new notimplementedexception(),
		//	};
		//}

		public static List<decimal> SimpleRegressionSim(StockInvestment stock, OptionsDict options) {
			var simOptions = new RegressionOptions() {
				BasePrice = stock.StockPrice,
				AnalysisLength = int.Parse(options["AnalysisLength"]),
			};

			var currentPrice = simOptions.BasePrice;
			var purchasePrice = simOptions.PurchasePrice;
			var quantity = stock.StockQuantity;
			var analysisLength = simOptions.AnalysisLength;
			int monthPurchased = stock.StockPurchaseDate.Month + stock.StockPurchaseDate.Year * 12;
			int monthCurrent = DateTime.Now.Month + DateTime.Now.Year * 12;

			var projectedDividend = ProjectStockDividend(stock, options);

			List<decimal> futurePrices = new List<decimal>();
			decimal slope = (currentPrice - purchasePrice) / (monthCurrent - monthPurchased);
			decimal intercept = currentPrice - slope * monthPurchased;
			decimal averageGrowth = slope / intercept;

			for (int i = 0; i < analysisLength; i++) {
				monthCurrent++;
				decimal price = intercept + (slope * monthCurrent);
				price += projectedDividend[i];
				futurePrices.Add(price);

			}
			return futurePrices;
		}

		public static List<decimal> SimpleRegressionSimWithSector(StockInvestment stock, OptionsDict options) {
			var simOptions = new RegressionOptions() {
				BasePrice = stock.StockPrice,
				AnalysisLength = int.Parse(options["AnalysisLength"]),
			};

			var currentPrice = simOptions.BasePrice;
			var purchasePrice = simOptions.PurchasePrice;
			var sector = stock.Sector;

			var quantity = stock.StockQuantity;
			var analysisLength = simOptions.AnalysisLength;
			int monthPurchased = stock.StockPurchaseDate.Month + stock.StockPurchaseDate.Year * 12;
			int monthCurrent = DateTime.Now.Month + DateTime.Now.Year * 12;

			var projectedDividend = ProjectStockDividend(stock, options);

			List<decimal> futurePrices = new List<decimal>();
			decimal slope = (currentPrice - purchasePrice) / (monthCurrent - monthPurchased);
			decimal intercept = currentPrice - slope * monthPurchased;
			decimal averageGrowth = slope / intercept;

			for (int i = 0; i < analysisLength; i++) {
				monthCurrent++;
				decimal price = intercept + (slope * monthCurrent);
				price += projectedDividend[i];
				futurePrices.Add(price);
			}
			return futurePrices;
		}

		private static int GetDividendIntervalMonths(string interval) => interval switch {
			"Month" => 1,
			"Quarter" => 3,
			"Annual" => 12,
			_ => throw new ArgumentException("Invalid Dividend Interval")
		};

		public static List<decimal> ProjectStockDividend(StockInvestment investment, OptionsDict options) {
			var quantityList = new List<decimal>();
			var stockQuantity = investment.StockQuantity;
			var dividendPercent = investment.StockDividendPercent;
			var currentMonth = DateTime.Now.Month;
			var firstDividendMonth = investment.StockDividendFirstPaymentDate.Month;
			var monthInterval = GetDividendIntervalMonths(investment.StockDividendDistributionInterval);

			//quantityList.Add(stockQuantity);
			for (int i = 0; i < int.Parse(options["AnalysisLength"]); i++) {
				if ((currentMonth - firstDividendMonth) % monthInterval == 0) {
					stockQuantity += stockQuantity * dividendPercent;
				}
				quantityList.Add(stockQuantity);
				currentMonth++;
				if (currentMonth > 12) {
					currentMonth = 1;
				}
			}

			return quantityList;
		}
	}
}
