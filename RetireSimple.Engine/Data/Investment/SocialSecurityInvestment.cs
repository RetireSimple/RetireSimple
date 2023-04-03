using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Base;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.Data.Investment {
	public class SocialSecurityInvestment : Base.Investment {

		[JsonIgnore, NotMapped]
		public DateOnly SocialSecurityStartDate {
			get => DateOnly.Parse(InvestmentData["SocialSecurityStartDate"]);
			set => InvestmentData["SocialSecurityStartDate"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public int SocialSecurityAge {
			get => int.Parse(InvestmentData["SocialSecurityAge"]);
			set => InvestmentData["SocialSecurityAge"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal SocialSecurityStartAmount {
			get => decimal.Parse(InvestmentData["SocialSecurityStartAmount"]);
			set => InvestmentData["SocialSecurityStartAmount"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal SocialSecurityYearlyIncrease {
			get => decimal.Parse(InvestmentData["SocialSecurityYearlyIncrease"]);
			set => InvestmentData["SocialSecurityYearlyIncrease"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public AnalysisModule<SocialSecurityInvestment>? AnalysisMethod { get; private set; }

		//Constructor used by EF
		public SocialSecurityInvestment(string analysisType) : base(analysisType) { }

		public override InvestmentModel InvokeAnalysis(OptionsDict options) =>
			AnalysisMethod is not null
			? AnalysisMethod(this, options)
			: throw new InvalidOperationException("The specified investment has no specified analysis");
	}

}
