namespace RetireSimple.Backend.Controllers.RequestBody {
	public class StockAddRequestBody {
		public string Ticker { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public DateTime Date { get; set; }
	}
}
