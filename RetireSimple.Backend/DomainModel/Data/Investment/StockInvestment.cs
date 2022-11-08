using RetireSimple.Backend.DomainModel.Analysis;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetireSimple.Backend.DomainModel.Data.Investment {

	public class StockInvestment : InvestmentBase {

		[NotMapped]
		public decimal StockPrice {
			get => decimal.Parse(this.InvestmentData["StockPrice"]);
			set => this.InvestmentData["StockPrice"] = value.ToString();
		}
		
		[NotMapped]
		public string StockTicker {
			get => this.InvestmentData["StockTicker"];
			set => this.InvestmentData["StockTicker"] = value;
		}
		
		[NotMapped]
		public int StockQuantity {
			get => int.Parse(this.InvestmentData["StockQuantity"]);
			set => this.InvestmentData["StockQuantity"] = value.ToString();
		}

		[NotMapped]
		public DateTime StockPurchaseDate {
			get => DateTime.Parse(this.InvestmentData["StockPurchaseDate"]);
			set => this.InvestmentData["StockPurchaseDate"] = value.ToString();
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