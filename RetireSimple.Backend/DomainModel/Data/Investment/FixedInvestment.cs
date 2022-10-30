using System.ComponentModel.DataAnnotations.Schema;

namespace RetireSimple.Backend.DomainModel.Data.Investment {

	public class FixedInvestment : InvestmentBase {
		[NotMapped]
		public double FixedValue { get => double.Parse(this.InvestmentData["FixedValue"]); set => this.InvestmentData["FixedValue"] = value.ToString(); }

		public AnalysisDelegate<FixedInvestment>? analysis;

		public override void ResolveAnalysisDelegate(string analysisType) => throw new NotImplementedException();
		public override InvestmentModel InvokeAnalysis() => throw new NotImplementedException();
	}
}