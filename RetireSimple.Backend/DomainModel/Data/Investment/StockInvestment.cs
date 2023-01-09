using RetireSimple.Backend.DomainModel.Analysis;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Backend.DomainModel.Data.Investment {

	public class StockInvestment : InvestmentBase {

		/// <summary>
		/// The price of the Stock
		/// </summary>
		[JsonIgnore]
		[NotMapped]
		public decimal StockPrice {
			get => decimal.Parse(this.InvestmentData["stockPrice"]);
			set => this.InvestmentData["stockPrice"] = value.ToString();
		}

		/// <summary>
		/// The Ticker Symbol of the Stock. Currently only used for identification purposes
		/// </summary>
		[JsonIgnore]
		[NotMapped]
		public string StockTicker {
			get => this.InvestmentData["stockTicker"];
			set => this.InvestmentData["stockTicker"] = value;
		}

		/// <summary>
		/// The number of shares of the stock held
		/// </summary>
		[JsonIgnore]
		[NotMapped]
		public decimal StockQuantity {
			get => decimal.Parse(this.InvestmentData["stockQuantity"]);
			set => this.InvestmentData["stockQuantity"] = value.ToString();
		}

		/// <summary>
		/// The date the stock was purchased. Stores as a Date-Time, but only the date is used. May change before formal release
		/// </summary>
		[JsonIgnore]
		[NotMapped]
		public DateTime StockPurchaseDate {
			get => DateTime.Parse(this.InvestmentData["stockPurchaseDate"]);
			set => this.InvestmentData["stockPurchaseDate"] = value.ToString();
		}

		/// <summary>
		/// Percentage dividend that the stock pays. This is a decimal value between 0 and 1.
		/// </summary>
		[JsonIgnore]
		[NotMapped]
		public decimal StockDividendPercent {
			get => decimal.Parse(this.InvestmentData["stockDividendPercent"]);
			set => this.InvestmentData["stockDividendPercent"] = value.ToString();
		}

		//TODO validation maybe?
		/// <summary>
		/// The interval upon which dividends are paid. Valid values are "Month", "Quarter", "Annual"
		/// </summary>
		[JsonIgnore]
		[NotMapped]
		public string StockDividendDistributionInterval {
			get => this.InvestmentData["stockDividendDistributionInterval"];
			set => this.InvestmentData["stockDividendDistributionInterval"] = value;
		}

		//TODO validation maybe?
		/// <summary>
		/// Method of distribution of the dividends. Valid Values are "Stock", "Cash", "DRIP"
		/// </summary>
		[JsonIgnore]
		[NotMapped]
		public string StockDividendDistributionMethod {
			get => this.InvestmentData["stockDividendDistributionMethod"];
			set => this.InvestmentData["stockDividendDistributionMethod"] = value;
		}

		/// <summary>
		/// A date to project the Dividend Payment months (Simplified to reduce information needed)
		/// </summary>
		[JsonIgnore]
		[NotMapped]
		public DateTime StockDividendFirstPaymentDate {
			get => DateTime.Parse(this.InvestmentData["stockDividendFirstPaymentDate"]);
			set => this.InvestmentData["stockDividendFirstPaymentDate"] = value.ToString();
		}

		[NotMapped]
		[JsonIgnore]
		public AnalysisDelegate<StockInvestment>? Analysis;

		//Constructor used by EF
		public StockInvestment(String analysisType) : base() {
			InvestmentType = "StockInvestment";
			ResolveAnalysisDelegate(analysisType);

		}

		public override void ResolveAnalysisDelegate(string analysisType) {
			switch(analysisType) {
				case "testAnalysis":
					Analysis = StockAS.testAnalysis;
					break;
				case "MonteCarlo_NormalDist":
					Analysis = StockAS.MonteCarlo_NormalDist;
					break;
				default:
					Analysis = null;
					break;

			}

			//Overwrite The current Analysis Delegate Type 
			this.AnalysisType = analysisType;
		}

		public override InvestmentModel InvokeAnalysis(OptionsDict options) => Analysis(this, options);
	}
}