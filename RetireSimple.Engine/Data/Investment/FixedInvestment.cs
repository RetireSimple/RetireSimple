using RetireSimple.Engine.Analysis;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.Data.Investment {

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
		public AnalysisModule<FixedInvestment>? AnalysisMethod { get; private set; }

		public override InvestmentModel InvokeAnalysis(OptionsDict options) =>
			AnalysisMethod is not null
			? AnalysisMethod(this, options)
			: throw new InvalidOperationException("The specified investment has no specified analysis");
	}
}