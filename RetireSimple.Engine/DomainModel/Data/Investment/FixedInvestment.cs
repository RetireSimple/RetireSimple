using RetireSimple.Engine.DomainModel.Analysis;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.DomainModel.Data.Investment {

	public class FixedInvestment : InvestmentBase {

		[JsonIgnore, NotMapped]
		public double FixedValue {
			get => double.Parse(InvestmentData["FixedValue"]);
			set => InvestmentData["FixedValue"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal FixedInterestedRate {
			get => decimal.Parse(InvestmentData["FixedInterestRate"]);
			set => InvestmentData["FixedInterestRate"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public AnalysisDelegate<FixedInvestment>? AnalysisMethod;

		public override void ResolveAnalysisDelegate(string analysisType) {
			switch (analysisType) {
				case "DefaultCashAnalysis":
					AnalysisMethod = FixedAS.DefaultFixedAnalyis;
					break;
				default:
					AnalysisMethod = null;
					break;
			}
			//Overwrite The current Analysis Delegate Type
			AnalysisType = analysisType;
		}
		public override InvestmentModel InvokeAnalysis(OptionsDict options) =>
			AnalysisMethod is not null
			? AnalysisMethod(this, options)
			: throw new InvalidOperationException("The specified investment has no specified analysis");
	}
}