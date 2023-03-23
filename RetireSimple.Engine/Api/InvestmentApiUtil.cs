using RetireSimple.Engine.Data.Investment;

namespace RetireSimple.Engine.Api {
	/// <summary>
	/// Contains static methods used by the InvestmentApi to create new investments.
	/// Is purely for organization purposes
	/// </summary>
	public static class InvestmentApiUtil {

		public static StockInvestment CreateStock(OptionsDict parameters) {
			var defaults = GetStockDefaults();
			var analysisType = parameters.GetValueOrDefault("analysisType") ?? "MonteCarlo_NormalDist";

			return new StockInvestment(analysisType) {
				StockPrice = decimal.Parse(parameters.GetValueOrDefault("stockPrice", defaults["stockPrice"])),
				StockTicker = parameters.GetValueOrDefault("stockTicker", defaults["stockTicker"]),
				StockQuantity = decimal.Parse(parameters.GetValueOrDefault("stockQuantity", defaults["stockQuantity"])),
				StockPurchaseDate = DateOnly.FromDateTime(DateTime.Parse(parameters.GetValueOrDefault("stockPurchaseDate", defaults["stockPurchaseDate"]))),
				StockDividendPercent = decimal.Parse(parameters.GetValueOrDefault("stockDividendPercent", defaults["stockDividendPercent"])),
				StockDividendDistributionInterval = parameters.GetValueOrDefault("stockDividendDistributionInterval", defaults["stockDividendDistributionInterval"]),
				StockDividendDistributionMethod = parameters.GetValueOrDefault("stockDividendDistributionMethod", defaults["stockDividendDistributionMethod"]),
				StockDividendFirstPaymentDate = DateOnly.FromDateTime(DateTime.Parse(parameters.GetValueOrDefault("stockDividendFirstPaymentDate", defaults["stockDividendFirstPaymentDate"]))),
			};
		}

		public static BondInvestment CreateBond(OptionsDict parameters) {
			var defaults = GetBondDefaults();
			var analysisType = parameters.GetValueOrDefault("analysisType") ?? "bondValuationAnalysis";

			return new BondInvestment(analysisType) {
				BondTicker = parameters.GetValueOrDefault("bondTicker", defaults["bondTicker"]),
				BondCouponRate = double.Parse(parameters.GetValueOrDefault("bondCouponRate", defaults["bondCouponRate"])),
				BondYTM = decimal.Parse(parameters.GetValueOrDefault("bondYieldToMaturity", defaults["bondYieldToMaturity"])),
				BondMaturityDate = DateOnly.FromDateTime(DateTime.Parse(parameters.GetValueOrDefault("bondMaturityDate", defaults["bondMaturityDate"]))),
				BondFaceValue = decimal.Parse(parameters.GetValueOrDefault("bondFaceValue", defaults["bondFaceValue"])),
				BondPurchaseDate = DateOnly.FromDateTime(DateTime.Parse(parameters.GetValueOrDefault("bondPurchaseDate", defaults["bondPurchaseDate"]))),
				BondCurrentPrice = decimal.Parse(parameters.GetValueOrDefault("bondCurrentPrice", defaults["bondCurrentPrice"])),
				// BondIsAnnual = parameters.GetValueOrDefault("bondIsAnnual", defaults["bondIsAnnual"]),
			};
		}

		///Default Values Methods for Investments
		public static OptionsDict GetStockDefaults() => new() {
			["stockPrice"] = "0",
			["stockTicker"] = "N/A",
			["stockQuantity"] = "0",
			["stockPurchaseDate"] = DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd"),
			["stockDividendPercent"] = "0",
			["stockDividendDistributionInterval"] = "Month",
			["stockDividendDistributionMethod"] = "Stock",
			["stockDividendFirstPaymentDate"] = DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd"),
		};

		public static OptionsDict GetBondDefaults() => new() {
			["bondTicker"] = "N/A",
			["bondCouponRate"] = "0.05",
			["bondYieldToMaturity"] = "0.05",
			["bondMaturityDate"] = DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd"),
			["bondFaceValue"] = "0",
			["bondPurchaseDate"] = DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd"),
			["bondCurrentPrice"] = "0",
		};
	}

}