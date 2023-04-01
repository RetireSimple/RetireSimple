using RetireSimple.Engine.Analysis;
using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Base;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.Data.Investment {
	public class AnnuityInvestment : Base.Investment {

		[JsonIgnore, NotMapped]
		public string AnnuityName {
			get => InvestmentData["AnnuityName"];
			set => InvestmentData["AnnuityName"] = value;
		}

		[JsonIgnore, NotMapped]
		public decimal AnnuityStartAmount {
			get => decimal.Parse(InvestmentData["AnnuityStartAmount"]);
			set => InvestmentData["AnnuityStartAmount"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public DateOnly AnnuityStartDate {
			get => DateOnly.Parse(InvestmentData["AnnuityStartDate"]);
			set => InvestmentData["AnnuityStartDate"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal AnnuityMonthlyPayment {
			get => decimal.Parse(InvestmentData["AnnuityMonthlyPayment"]);
			set => InvestmentData["AnnuityMonthlyPayment"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal AnnuityYield {
			get => decimal.Parse(InvestmentData["AnnuityYield"]);
			set => InvestmentData["AnnuityYield"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public AnalysisModule<AnnuityInvestment>? AnalysisMethod { get; private set; }

		public AnnuityInvestment(string analysisType) : base(analysisType) { }

		public override InvestmentModel InvokeAnalysis(OptionsDict options) =>
			AnalysisMethod is not null
			? AnalysisMethod(this, options)
			: throw new InvalidOperationException("The specified investment has no specified analysis");
	}
}

