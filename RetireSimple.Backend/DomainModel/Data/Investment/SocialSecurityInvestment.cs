using RetireSimple.Backend.DomainModel.Analysis;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Backend.DomainModel.Data.Investment

{
	public class SocialSecurityInvestment : InvestmentBase {

		[JsonIgnore]
		[NotMapped]
		public decimal StartAmount {
			get => decimal.Parse(this.InvestmentData["SSStartAmount"]);
			set => this.InvestmentData["SSStartAmount"] = value.ToString();
		}

		[JsonIgnore]
		[NotMapped]
		public DateTime StartDate {
			get => DateTime.Parse(this.InvestmentData["SSStartDate"]);
			set => this.InvestmentData["SSStartDate"] = value.ToString();
		}

		[JsonIgnore]
		[NotMapped]
		public decimal YearlyIncrease {
			get => decimal.Parse(this.InvestmentData["SSYearlyIncrease"]);
			set => this.InvestmentData["SSYearlyIncrease"] = value.ToString();
		}

		[JsonIgnore]
		[NotMapped]
		public String StartAge {
			get => this.InvestmentData["SSStartAge"];
			set => this.InvestmentData["SSStartAge"] = value;
		}

		public AnalysisDelegate<SocialSecurityInvestment>? analysis;

		public override void ResolveAnalysisDelegate(string analysisType)
		{
			switch(analysisType) {
				case "DefaultSSAnalysis":
					this.analysis = SSAS.DefaultSSAnalyis;
					break;
				default:
					this.analysis = null;
					break;
			}
			//Overwrite The current Analysis Delegate Type
			this.AnalysisType = analysisType;
		}

		public override InvestmentModel InvokeAnalysis(OptionsDict options) => analysis(this, options);
	}
}

