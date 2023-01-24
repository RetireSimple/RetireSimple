using RetireSimple.Backend.DomainModel.Analysis;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace RetireSimple.Backend.DomainModel.Data.Investment {
	public class CashInvestment : InvestmentBase {

		[JsonIgnore, NotMapped]
		public string CashCurrency {
			get => this.InvestmentData["CashCurrency"];
			set => this.InvestmentData["CashCurrency"] = value;
		}

		[JsonIgnore, NotMapped]
		public decimal CashQuantity {
			get => decimal.Parse(this.InvestmentData["CashQuantity"]);
			set => this.InvestmentData["CashQuantity"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal CashCurrentValue {
			get => decimal.Parse(this.InvestmentData["CashValue"]);
			set => this.InvestmentData["CashValue"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public AnalysisDelegate<CashInvestment>? AnalysisMethod;

		public CashInvestment(String analysisType) : base() {
			InvestmentType = "CashInvestment";
			ResolveAnalysisDelegate(analysisType);
		}

		public override void ResolveAnalysisDelegate(string analysisType) {
			switch(analysisType) {
				case "DefaultCashAnalysis":
					this.AnalysisMethod = CashAS.DefaultCashAnalysis;
					break;
				default:
					this.AnalysisMethod = null;
					break;
			}
			//Overwrite The current Analysis Delegate Type
			this.AnalysisType = analysisType;
		}
		public override InvestmentModel InvokeAnalysis(OptionsDict options) =>
			(AnalysisMethod is not null)
			? AnalysisMethod(this, options)
			: throw new InvalidOperationException("The specified investment has no specified analysis");
	}
}
