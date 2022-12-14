using System;
using RetireSimple.Backend.DomainModel.Analysis;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Backend.DomainModel.Data.Investment {

	public class AnnuityInvestment : InvestmentBase
	{
		[JsonIgnore]
		[NotMapped]
		public decimal StartAmount
		{
			get => decimal.Parse(this.InvestmentData["AnnuityStartAmount"]);
			set => this.InvestmentData["AnnuityStartAmount"] = value.ToString();
		}

		[JsonIgnore]
		[NotMapped]
		public DateTime StartDate {
			get => DateTime.Parse(this.InvestmentData["AnnuityStartDate"]);
			set => this.InvestmentData["AnnuityStartDate"] = value.ToString();
		}

		[JsonIgnore]
		[NotMapped]
		public decimal MonthlyPayment {
			get => decimal.Parse(this.InvestmentData["AnnuityMonthlyPayment"]);
			set => this.InvestmentData["AnnuityMonthlyPayment"] = value.ToString();
		}

		[JsonIgnore]
		[NotMapped]
		public decimal InterestRate {
			get => decimal.Parse(this.InvestmentData["AnnuityInterestRate"]);
			set => this.InvestmentData["AnnuityInterestRate"] = value.ToString();
		}

		public AnalysisDelegate<AnnuityInvestment>? analysis;

		public override void ResolveAnalysisDelegate(string analysisType) {
			switch(analysisType) {
				case "DefaultAnnuityAnalysis":
					this.analysis = AnnuityAS.DefaultAnnuityAnalyis;
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

