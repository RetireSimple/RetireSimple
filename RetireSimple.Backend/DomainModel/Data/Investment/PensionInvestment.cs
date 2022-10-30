namespace RetireSimple.Backend.DomainModel.Data.Investment {
	public class PensionInvestment : InvestmentBase {
		public AnalysisDelegate<PensionInvestment>? analysis;

		public override InvestmentModel InvokeAnalysis() => throw new NotImplementedException();
		public override void ResolveAnalysisDelegate(string analysisType) => throw new NotImplementedException();
	}
}