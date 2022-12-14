
﻿using RetireSimple.Backend.DomainModel.Analysis;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Backend.DomainModel.Data.Investment {
	public class SocialSecurityInvestment : InvestmentBase {

		[JsonIgnore]
		[NotMapped]
		public DateOnly SocialSecurityStartDate {
			get => DateOnly.Parse(this.InvestmentData["SocialSecurityStartDate"]);
			set => this.InvestmentData["SocialSecurityStartDate"] = value.ToString();
		}
		
		[JsonIgnore]
		[NotMapped]
		public int SocialSecurityAge {
			get => int.Parse(this.InvestmentData["SocialSecurityAge"]);
			set => this.InvestmentData["SocialSecurityAge"] = value.ToString();
		}

		[JsonIgnore]
		[NotMapped]
		public decimal SocialSecurityStartAmount {
			get => decimal.Parse(this.InvestmentData["SocialSecurityStartAmount"]);
			set => this.InvestmentData["SocialSecurityStartAmount"] = value.ToString();
		}

		[JsonIgnore]
		[NotMapped]
		public decimal SocialSecurityYearlyIncrease {
			get => decimal.Parse(this.InvestmentData["SocialSecurityYearlyIncrease"]);
			set => this.InvestmentData["SocialSecurityYearlyIncrease"] = value.ToString();
		}

		[NotMapped]
		[JsonIgnore]
		public AnalysisDelegate<SocialSecurityInvestment>? Analysis;

		//Constructor used by EF
		public SocialSecurityInvestment(String analysisType) : base() {
			InvestmentType = "SocialSecurityInvestment";
			ResolveAnalysisDelegate(analysisType);
		}

		public override void ResolveAnalysisDelegate(string analysisType) {
			switch(analysisType) {
				case "testAnalysis":
					Analysis = SocialSecurityAS.DefaultSocialSecurityAnalysis;
					break;
				default:
					Analysis = null;
					break;

			}
			//Overwrite The current Analysis Delegate Type 
			this.AnalysisType = analysisType;
		}

		public override InvestmentModel InvokeAnalysis(OptionsDict options) => Analysis(this, options);
	}

}
