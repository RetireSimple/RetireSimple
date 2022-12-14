using RetireSimple.Backend.DomainModel.Analysis;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Backend.DomainModel.Data.Investment {

	public class FixedInvestment : InvestmentBase {

		[JsonIgnore]
		[NotMapped]
		public double FixedValue {
			get => double.Parse(this.InvestmentData["FixedValue"]);
			set => this.InvestmentData["FixedValue"] = value.ToString();
		}

		public decimal FixedInterestedRate
		{
			get => decimal.Parse(this.InvestmentData["FixedInterestRate"]);
			set => this.InvestmentData["FixedInterestRate"] = value.ToString();
		}



		public AnalysisDelegate<FixedInvestment>? analysis;

		public override void ResolveAnalysisDelegate(string analysisType)
		{
			switch(analysisType) {
				case "DefaultCashAnalysis":
					this.analysis = FixedAS.DefaultFixedAnalyis;
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