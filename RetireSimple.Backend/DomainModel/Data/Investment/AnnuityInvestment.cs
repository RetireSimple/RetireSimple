using RetireSimple.Backend.DomainModel.Analysis;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Backend.DomainModel.Data.Investment {
	public class AnnuityInvestment : InvestmentBase {

		[JsonIgnore, NotMapped]
		public string AnnuityName {
			get => this.InvestmentData["AnnuityName"];
			set => this.InvestmentData["AnnuityName"] = value;
		}

		[JsonIgnore, NotMapped]
		public decimal AnnuityStartAmount {
			get => decimal.Parse(this.InvestmentData["AnnuityStartAmount"]);
			set => this.InvestmentData["AnnuityStartAmount"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public DateOnly AnnuityStartDate {
			get => DateOnly.Parse(this.InvestmentData["AnnuityStartDate"]);
			set => this.InvestmentData["AnnuityStartDate"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal AnnuityMonthlyPayment {
			get => decimal.Parse(this.InvestmentData["AnnuityMonthlyPayment"]);
			set => this.InvestmentData["AnnuityMonthlyPayment"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal AnnuityYield {
			get => decimal.Parse(this.InvestmentData["AnnuityYield"]);
			set => this.InvestmentData["AnnuityYield"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public AnalysisDelegate<AnnuityInvestment>? AnalysisMethod;

		public AnnuityInvestment(String analysisType) : base() {
			InvestmentType = "AnnuityInvestment";
			ResolveAnalysisDelegate(analysisType);
		}

		public override void ResolveAnalysisDelegate(string analysisType) {
			switch(analysisType) {
				case "DefaultCashAnalysis":
					this.AnalysisMethod = AnnuityAS.DefaultAnnuityAnalyis;
					break;
				default:
					this.AnalysisMethod = null;
					break;
			}
			//Overwrite The current Analysis Delegate Type
			this.AnalysisType = analysisType;
		}
		public override InvestmentModel InvokeAnalysis(OptionsDict options) =>
			(AnalysisMethod is not null)
			? AnalysisMethod(this, options)
			: throw new InvalidOperationException("The specified investment has no specified analysis");
	}
}

