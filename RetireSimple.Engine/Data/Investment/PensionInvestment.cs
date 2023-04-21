using Microsoft.Extensions.Options;

using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Base;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.Data.Investment {
	[InvestmentModule(nameof(AnalysisMethod))]
	public class PensionInvestment : Base.Investment {

		[JsonIgnore, NotMapped]
		public DateOnly PensionStartDate {
			get => DateOnly.Parse(InvestmentData["pensionStartDate"]);
			set => InvestmentData["pensionStartDate"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal PensionInitialMonthlyPayment {
			get => decimal.Parse(InvestmentData["pensionInitialMonthlyPayment"]);
			set => InvestmentData["pensionInitialMonthlyPayment"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal PensionYearlyIncrease {
			get => decimal.Parse(InvestmentData["pensionYearlyIncrease"]);
			set => InvestmentData["pensionYearlyIncrease"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public AnalysisModule<PensionInvestment>? AnalysisMethod { get; private set; }

		//Constructor used by EF
		public PensionInvestment(string analysisType) : base(analysisType) { }

		public PensionInvestment(OptionsDict parameters)
			: base(parameters.GetValueOrDefault("analysisType") ?? "PensionSimulation") {
			PensionInitialMonthlyPayment = decimal.Parse(parameters.GetValueOrDefault("pensionInitialMonthlyPayment", "0"));
			PensionStartDate = DateOnly.Parse(parameters.GetValueOrDefault("pensionStartDate", "1/1/2021"));
			PensionYearlyIncrease = decimal.Parse(parameters.GetValueOrDefault("pensionYearlyIncrease", "0"));
		}

		public override InvestmentModel InvokeAnalysis(OptionsDict options) =>
		AnalysisMethod is not null
		? AnalysisMethod(this, options)
		: throw new InvalidOperationException("The specified investment has no specified analysis");
	}
}