using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Backend.DomainModel.Data.Investment {
	public class PensionInvestment : InvestmentBase {
		public AnalysisDelegate<PensionInvestment>? analysis;

		[JsonIgnore]
		[NotMapped]
		public DateOnly StartDate {
			get => DateOnly.Parse(this.InvestmentData["PensionStartDate"]);
			set => this.InvestmentData["PensionStartDate"] = value.ToString();
		}

		public decimal InitialMonthlyPayment {
			get => decimal.Parse(this.InvestmentData["PensionInitialMonthlyPayment"]);
			set => this.InvestmentData["PensionInitialMonthlyPayment"] = value.ToString();
		}


		public override InvestmentModel InvokeAnalysis(OptionsDict options) => throw new NotImplementedException();
		public override void ResolveAnalysisDelegate(string analysisType) => throw new NotImplementedException();
	}
}