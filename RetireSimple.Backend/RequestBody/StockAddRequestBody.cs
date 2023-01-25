namespace RetireSimple.Backend.RequestBody {
	public class StockAddRequestBody {
		public string Name { get; set; }
		public string Ticker { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		//public DateTime Date { get; set; }
		public string AnalysisType { get; set; }
	}
}
