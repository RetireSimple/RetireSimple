using RetireSimple.Backend.DomainModel.Analysis;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Backend.DomainModel.Data.Investment {
	public class PensionInvestment : InvestmentBase {

		[JsonIgnore]
		[NotMapped]
		public string PensionName {
			get => this.InvestmentData["PensionName"];
			set => this.InvestmentData["PensionName"] = value;
		}

		[JsonIgnore]
		[NotMapped]
		public DateOnly PensionStartDate {
			get => DateOnly.Parse(this.InvestmentData["PensionStartDate"]);
			set => this.InvestmentData["PensionStartDate"] = value.ToString();
		}

		[JsonIgnore]
		[NotMapped]
		public decimal PensionInitialMonthlyPayment {
			get => decimal.Parse(this.InvestmentData["PensionInitialMonthlyPayment"]);
			set => this.InvestmentData["PensionInitialMonthlyPayment"] = value.ToString();
		}

		[JsonIgnore]
		[NotMapped]
		public decimal PensionYearlyIncrease {
			get => decimal.Parse(this.InvestmentData["PensionYearlyIncrease"]);
			set => this.InvestmentData["PensionYearlyIncrease"] = value.ToString();
		}

		public AnalysisDelegate<PensionInvestment>? analysis;

		//Constructor used by EF
		public PensionInvestment(String analysisType) : base() {
			InvestmentType = "PensionInvestment";
			ResolveAnalysisDelegate(analysisType);
		}
		public override void ResolveAnalysisDelegate(string analysisType) {
			switch(analysisType) {
				case "DefaultCashAnalysis":
					analysis = PensionAS.DefaultPensionAnalysis;
					break;
				default:
					analysis = null;
					break;
			}
			//Overwrite The current Analysis Delegate Type
			this.AnalysisType = analysisType;
		}

		public override InvestmentModel InvokeAnalysis(OptionsDict options) => throw new NotImplementedException();
	}
}