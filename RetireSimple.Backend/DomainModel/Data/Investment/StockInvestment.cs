using RetireSimple.Backend.DomainModel.Analysis;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetireSimple.Backend.DomainModel.Data.Investment {

	public class StockInvestment : InvestmentBase {

		[NotMapped]
		public decimal StockPrice {
			get => decimal.Parse(this.InvestmentData["stockPrice"]);
			set => this.InvestmentData["stockPrice"] = value.ToString();
		}
		
		[NotMapped]
		public string StockTicker {
			get => this.InvestmentData["stockTicker"];
			set => this.InvestmentData["stockTicker"] = value;
		}
		
		[NotMapped]
		public int StockQuantity {
			get => int.Parse(this.InvestmentData["stockQuantity"]);
			set => this.InvestmentData["stockQuantity"] = value.ToString();
		}

		[NotMapped]
		public DateTime StockPurchaseDate {
			get => DateTime.Parse(this.InvestmentData["stockPurchaseDate"]);
			set => this.InvestmentData["stockPurchaseDate"] = value.ToString();
		}

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
				case "testAnalysis2":
					Analysis = StockAS.testAnalysis2;
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