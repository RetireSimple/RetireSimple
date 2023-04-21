using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Base;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.Data.Investment {
	[InvestmentModule(nameof(AnalysisMethod))]
	public class PensionInvestment : Base.Investment {

		[JsonIgnore, NotMapped]
		public DateOnly PensionStartDate {
			get => DateOnly.Parse(InvestmentData["PensionStartDate"]);
			set => InvestmentData["PensionStartDate"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal PensionInitialMonthlyPayment {
			get => decimal.Parse(InvestmentData["PensionInitialMonthlyPayment"]);
			set => InvestmentData["PensionInitialMonthlyPayment"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal PensionYearlyIncrease {
			get => decimal.Parse(InvestmentData["PensionYearlyIncrease"]);
			set => InvestmentData["PensionYearlyIncrease"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public AnalysisModule<PensionInvestment>? AnalysisMethod { get; private set; }

		//Constructor used by EF
		public PensionInvestment(string analysisType) : base(analysisType) { }

		public override InvestmentModel InvokeAnalysis(OptionsDict options) =>
		AnalysisMethod is not null
		? AnalysisMethod(this, options)
		: throw new InvalidOperationException("The specified investment has no specified analysis");
	}
}