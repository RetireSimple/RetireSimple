using RetireSimple.Backend.DomainModel.Analysis;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetireSimple.Backend.DomainModel.Data.Investment {

	public class StockInvestment : InvestmentBase {

		[NotMapped]
		public double StockPrice { get => double.Parse(this.InvestmentData["StockPrice"]); set => this.InvestmentData["StockPrice"] = value.ToString(); }
		[NotMapped]
		public string StockTicker { get => this.InvestmentData["StockTicker"]; set => this.InvestmentData["StockTicker"] = value; }

		public AnalysisDelegate<StockInvestment>? analysis;

		//Constructor used by EF
		public StockInvestment(String analysisType) : base() {
			InvestmentType = "StockInvestment";
			ResolveAnalysisDelegate(analysisType);

		}


		public override void ResolveAnalysisDelegate(string analysisType) {
			switch(analysisType) {
				case "testAnalysis":
					analysis = StockAS.testAnalysis;
					break;
				case "testAnalysis2":
					analysis = StockAS.testAnalysis2;
					break;
				default:
					analysis = null;
					break;

			}

			//Overwrite The current Analysis Delegate Type 
			this.AnalysisType = analysisType;
		}

		public override void ValidateData() => throw new NotImplementedException();
		public override InvestmentModel InvokeAnalysis() => analysis(this);
	}




}