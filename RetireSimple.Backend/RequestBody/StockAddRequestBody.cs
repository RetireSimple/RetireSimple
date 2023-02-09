namespace RetireSimple.Backend.RequestBody {
	public class StockAddRequestBody {
		//For simplicity, strings are cast to their respective types
		public string InvestmentType { get; set; }
		public string InvestmentName { get; set; }
		public string StockTicker { get; set; }
		public string StockQuantity { get; set; }
		public string StockPrice { get; set; }
		//public DateTime Date { get; set; }
		public string StockPurchaseDate { get; set; }
		public string AnalysisType { get; set; }
		public string StockDividendPercent { get; set; }
		public string StockDividendDistributionInterval { get; set; }
		public string StockDividendDistributionMethod { get; set; }
		public string StockDividendFirstPaymentDate { get; set; }
	}
}
