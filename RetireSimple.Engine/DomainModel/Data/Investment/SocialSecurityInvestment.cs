using RetireSimple.Engine.DomainModel.Analysis;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.DomainModel.Data.Investment {
	public class SocialSecurityInvestment : InvestmentBase {

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
		public AnalysisDelegate<SocialSecurityInvestment>? AnalysisMethod;

		//Constructor used by EF
		public SocialSecurityInvestment(string analysisType) : base() {
			InvestmentType = "SocialSecurityInvestment";
			ResolveAnalysisDelegate(analysisType);
		}

		public override void ResolveAnalysisDelegate(string analysisType) {
			switch (analysisType) {
				case "testAnalysis":
					AnalysisMethod = SocialSecurityAS.DefaultSocialSecurityAnalysis;
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
