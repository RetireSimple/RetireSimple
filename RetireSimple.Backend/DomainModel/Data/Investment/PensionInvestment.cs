
using RetireSimple.Backend.DomainModel.Analysis;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Backend.DomainModel.Data.Investment {
	public class PensionInvestment : InvestmentBase {

		[JsonIgnore, NotMapped]
		public DateOnly PensionStartDate {
			get => DateOnly.Parse(this.InvestmentData["PensionStartDate"]);
			set => this.InvestmentData["PensionStartDate"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal PensionInitialMonthlyPayment {
			get => decimal.Parse(this.InvestmentData["PensionInitialMonthlyPayment"]);
			set => this.InvestmentData["PensionInitialMonthlyPayment"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal PensionYearlyIncrease {
			get => decimal.Parse(this.InvestmentData["PensionYearlyIncrease"]);
			set => this.InvestmentData["PensionYearlyIncrease"] = value.ToString();
		}

		public AnalysisDelegate<PensionInvestment>? AnalysisMethod;

		//Constructor used by EF
		public PensionInvestment(String analysisType) : base() {
			InvestmentType = "PensionInvestment";
			ResolveAnalysisDelegate(analysisType);
		}
		public override void ResolveAnalysisDelegate(string analysisType) {
			switch(analysisType) {
				case "DefaultCashAnalysis":
					AnalysisMethod = PensionAS.DefaultPensionAnalysis;
					break;
				default:
					AnalysisMethod = null;
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