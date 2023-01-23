using RetireSimple.Backend.DomainModel.Analysis;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Backend.DomainModel.Data.Investment {

	public class FixedInvestment : InvestmentBase {

		[JsonIgnore, NotMapped]
		public double FixedValue {
			get => double.Parse(this.InvestmentData["FixedValue"]);
			set => this.InvestmentData["FixedValue"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal FixedInterestedRate {
			get => decimal.Parse(this.InvestmentData["FixedInterestRate"]);
			set => this.InvestmentData["FixedInterestRate"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public AnalysisDelegate<FixedInvestment>? AnalysisMethod;

		public override void ResolveAnalysisDelegate(string analysisType) {
			switch(analysisType) {
				case "DefaultCashAnalysis":
					this.AnalysisMethod = FixedAS.DefaultFixedAnalyis;
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